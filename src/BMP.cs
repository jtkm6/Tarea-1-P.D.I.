using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System;

namespace PDI_Tarea_1 {
	class BMP {

		[StructLayout(LayoutKind.Explicit, Size = 14)]
		struct BITMAPFILEHEADER {
			[FieldOffset(0)]
			public ushort bfType;          //specifies the file type
			[FieldOffset(2)]
			public uint bfSize;            //specifies the size in bytes of the bitmap file
			[FieldOffset(6)]
			public ushort bfReserved1;     //reserved; must be 0
			[FieldOffset(8)]
			public ushort bfReserved2;     //reserved; must be 0
			[FieldOffset(10)]
			public uint bOffBits;			//species the offset in bytes from the bitmapfileheader to the bitmap bits
		};

		[StructLayout(LayoutKind.Explicit, Size = 40)]
		struct BITMAPINFOHEADER {
			[FieldOffset(0)]
			public uint biSize;            //specifies the number of bytes required by the struct
			[FieldOffset(4)]
			public int biWidth;            //specifies width in pixels
			[FieldOffset(8)]
			public int biHeight;           //species height in pixels
			[FieldOffset(12)]
			public ushort biPlanes;        //specifies the number of color planes, must be 1
			[FieldOffset(14)]
			public ushort biBitCount;      //specifies the number of bit per pixel
			[FieldOffset(16)]
			public uint biCompression;     //spcifies the type of compression
			[FieldOffset(20)]
			public uint biSizeImage;       //size of image in bytes
			[FieldOffset(24)]
			public int biXPelsPerMeter;    //number of pixels per meter in x axis
			[FieldOffset(28)]
			public int biYPelsPerMeter;    //number of pixels per meter in y axis
			[FieldOffset(32)]
			public uint biClrUsed;         //number of colors used by th ebitmap
			[FieldOffset(36)]
			public uint biClrImportant;		//number of colors that are important
		};

		private BITMAPFILEHEADER fileHeader;
		private BITMAPINFOHEADER imageHeader;
		private byte[] colorTable;
		private byte[] imageData;
		private string imagePatch;
		private Bitmap imagePreview;

		private T ByteArrayToStructure<T>(byte[] bytes) where T : struct {
			GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
			handle.Free();
			return stuff;
		}

		private byte[] StructureToByteArray<T>(T stuff) where T : struct {
			int objsize = Marshal.SizeOf(typeof(T));
			Byte[] ret = new Byte[objsize];
			IntPtr buff = Marshal.AllocHGlobal(objsize);
			Marshal.StructureToPtr(stuff, buff, true);
			Marshal.Copy(buff, ret, 0, objsize);
			Marshal.FreeHGlobal(buff);
			return ret;
		}

		private byte[] GetBytesFileData() {
			this.fileHeader.bfSize = (uint)14 + (uint)40 + (uint)colorTable.Length + (uint)imageData.Length;
			this.fileHeader.bOffBits = (uint)14 + (uint)40 + (uint)colorTable.Length;
			this.imageHeader.biSize = 40;
			this.imageHeader.biSizeImage = (uint)imageData.Length;
			this.imageHeader.biClrUsed = (uint)(colorTable.Length / 4);
			byte[] fileHeader = StructureToByteArray<BITMAPFILEHEADER>(this.fileHeader);
			byte[] imageHeader = StructureToByteArray<BITMAPINFOHEADER>(this.imageHeader);
			byte[] outFileData = new byte[fileHeader.Length + imageHeader.Length + colorTable.Length + imageData.Length];
			System.Buffer.BlockCopy(fileHeader, 0, outFileData, 0, fileHeader.Length);
			System.Buffer.BlockCopy(imageHeader, 0, outFileData, fileHeader.Length, imageHeader.Length);
			System.Buffer.BlockCopy(colorTable, 0, outFileData, fileHeader.Length + imageHeader.Length, colorTable.Length);
			System.Buffer.BlockCopy(imageData, 0, outFileData, fileHeader.Length + imageHeader.Length + colorTable.Length, imageData.Length);
			return outFileData;
		}

		private void WritePixel(byte[] imagen, int x, int y, uint pixel, int Width = 0) {
			int byteToRead, bytesPerRow, bytesUsedPerRow, bytesPaddingPerRow, pixelsPerRow = (Width == 0)? imageHeader.biWidth : Width;
			switch(imageHeader.biBitCount) {
				case 1:
					bytesUsedPerRow = (int)Math.Ceiling((double)pixelsPerRow / 8.0);
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0)? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					byteToRead = (y * bytesPerRow) + x / 8;
					int bitToRead = (7 - (x % 8));
					imagen[byteToRead] = (byte)(((byte)pixel << bitToRead) | (~(0x1 << bitToRead) & imagen[byteToRead]));
					break;
				case 4:
					bytesUsedPerRow = (int)Math.Ceiling((double)pixelsPerRow / 2.0);
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					byteToRead = (y * bytesPerRow) + x / 2;
					int bitesToRead = ((x % 2) == 0)? 4 : 0;
					imagen[byteToRead] = (byte)(((byte)pixel << bitesToRead) | (~(0xF << bitesToRead) & imagen[byteToRead]));
					break;
				case 8:
					bytesUsedPerRow = pixelsPerRow;
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					byteToRead = (y * bytesPerRow) + x;
					imagen[byteToRead] = (byte)(0xFF & pixel);
					break;
				default:
					bytesUsedPerRow = pixelsPerRow * 3;
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					byteToRead = (y * bytesPerRow) + (x * 3);
					imagen[byteToRead + 2] = (byte)(0xFF & pixel);
					imagen[byteToRead + 1] = (byte)(0xFF & (pixel >> 8));
					imagen[byteToRead] = (byte)(0xFF & (pixel >> 16));
					break;
			}
			
		}

		private uint ReadPixel(byte[] imagen, int x, int y) {
			int byteToRead, bytesPerRow, bytesUsedPerRow, bytesPaddingPerRow, pixelsPerRow = imageHeader.biWidth;
			switch(imageHeader.biBitCount) {
				case 1:
					bytesUsedPerRow = (int)Math.Ceiling((double)pixelsPerRow / 8.0);
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					byteToRead = (y * bytesPerRow) + x / 8;
					int bitToRead = (7 - (x % 8));
					return (uint)(0x1 & (imagen[byteToRead] >> bitToRead));
				case 4:
					bytesUsedPerRow = (int)Math.Ceiling((double)pixelsPerRow / 2.0);
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					byteToRead = (y * bytesPerRow) + x / 2;
					int bitesToRead = ((x % 2) == 0) ? 4 : 0;
					return (uint)(0xF & (imagen[byteToRead] >> bitesToRead));
				case 8:
					bytesUsedPerRow = pixelsPerRow;
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					byteToRead = (y * bytesPerRow) + x;
					return (uint)(0xFF & imagen[byteToRead]);
				default:
					bytesUsedPerRow = pixelsPerRow * 3;
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					byteToRead = (y * bytesPerRow) + (x * 3);
					return (uint)((0xFF0000 & (imagen[byteToRead] << 16)) | (0x00FF00 & (imagen[byteToRead + 1] << 8)) | (0x0000FF & imagen[byteToRead + 2]));
			}
		}

		public void LoadImageFile(string fileName){
			try {
				using(BinaryReader bin = new BinaryReader(File.Open(fileName, FileMode.Open))) {
					// Cargamos la informacion de la cabecera en la estructura.
					fileHeader = ByteArrayToStructure<BITMAPFILEHEADER>(bin.ReadBytes(14));
					imageHeader = ByteArrayToStructure<BITMAPINFOHEADER>(bin.ReadBytes(40));

					// Verificamos que tenga el formato con el que vamos a trabajar.
					if(fileHeader.bfType == 0x4D42 && imageHeader.biCompression == 0 && (imageHeader.biBitCount == 1 || imageHeader.biBitCount == 4 || imageHeader.biBitCount == 8 || imageHeader.biBitCount == 24)) {	
						imageData = new byte[imageHeader.biSizeImage];
						if(imageHeader.biBitCount != 24)
							imageHeader.biClrUsed = (uint)(0x1 << imageHeader.biBitCount);
						bin.BaseStream.Seek(imageHeader.biSize + 14, SeekOrigin.Begin);
						colorTable = bin.ReadBytes((int)imageHeader.biClrUsed * 4);
						bin.BaseStream.Seek(fileHeader.bOffBits, SeekOrigin.Begin);
						imageData = bin.ReadBytes((int)imageHeader.biSizeImage);
						imagePatch = fileName;
					}
					bin.Dispose();
					bin.Close();
				}
			} catch(Exception _Exception) {
				// Error
				MessageBox.Show(_Exception.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void SaveImageFile() {
			try {
				// Open file for reading
				System.IO.FileStream _FileStream = new System.IO.FileStream(imagePatch, System.IO.FileMode.Create, System.IO.FileAccess.Write);

				// Writes a block of bytes to this stream using data from a byte array.
				byte[] data = GetBytesFileData();
				_FileStream.Write(data, 0, data.Length);

				// close file stream
				_FileStream.Close();
			} catch(Exception _Exception) {
				// Error
				MessageBox.Show(_Exception.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public Bitmap GetImage() {
			if(imagePreview != null) imagePreview.Dispose();
			using(var ms = new MemoryStream(GetBytesFileData())) {
				imagePreview = new Bitmap(ms);
				return imagePreview;
			}
		}

		public bool ImageIsLoaded() {
			return imagePatch != null;
		}

		public void FlipVertical() {
			for(int y = 0; y < imageHeader.biHeight / 2; ++y) {
				for(int x = 0; x < imageHeader.biWidth; ++x) {
					uint pixel_1 = ReadPixel(imageData, x, y), pixel_2 = ReadPixel(imageData, x, (imageHeader.biHeight - 1) - y);
					WritePixel(imageData, x, y, pixel_2);
					WritePixel(imageData, x, (imageHeader.biHeight - 1) - y, pixel_1);
				}
			}
		}

		public void FlipHorizontal() {
			for(int y = 0; y < imageHeader.biHeight; ++y) {
				for(int x = 0; x < imageHeader.biWidth / 2; ++x) {
					uint pixel_1 = ReadPixel(imageData, x, y), pixel_2 = ReadPixel(imageData, (imageHeader.biWidth - 1) - x, y);
					WritePixel(imageData, x, y, pixel_2);
					WritePixel(imageData, (imageHeader.biWidth - 1) - x, y, pixel_1);
				}
			}
		}

		public void InvertirColores() {
			if(imageHeader.biBitCount == 8 || imageHeader.biBitCount == 4 || imageHeader.biBitCount == 1) {
				for(int i = 0; i < colorTable.Length; i += 4) {
					colorTable[i] = (byte)~colorTable[i];
					colorTable[i + 1] = (byte)~colorTable[i + 1];
					colorTable[i + 2] = (byte)~colorTable[i + 2];
				}
			}else{
				for(int i = 0; i < imageHeader.biHeight; ++i) {
					for(int j = 0; j < imageHeader.biWidth; ++j) {
						WritePixel(imageData, j, i, ~ReadPixel(imageData, j, i));
					}
				}
			}
		}

		public void Girar90GradosIzquierda() {
			int sizeOfNewImage = 0, bytesPerRow, bytesUsedPerRow, bytesPaddingPerRow, pixelsPerRow = imageHeader.biHeight;
			switch(imageHeader.biBitCount) {
				case 1:
					bytesUsedPerRow = (int)Math.Ceiling((double)pixelsPerRow / 8.0);
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					sizeOfNewImage = bytesPerRow * imageHeader.biWidth;
					break;
				case 4:
					bytesUsedPerRow = (int)Math.Ceiling((double)pixelsPerRow / 2.0);
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					sizeOfNewImage = bytesPerRow * imageHeader.biWidth;
					break;
				case 8:
					bytesUsedPerRow = pixelsPerRow;
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					sizeOfNewImage = bytesPerRow * imageHeader.biWidth;
					break;
				case 24:
					bytesUsedPerRow = pixelsPerRow * 3;
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					sizeOfNewImage = bytesPerRow * imageHeader.biWidth;
					break;
			}
			byte[] newImageData = new byte[sizeOfNewImage];
			for(int y = 0; y < imageHeader.biHeight; ++y) {
				for(int x = 0; x < imageHeader.biWidth; ++x) {
					WritePixel(newImageData, y, (imageHeader.biWidth - 1) - x, ReadPixel(imageData, x, y), imageHeader.biHeight);
				}
			}
			imageData = newImageData;
			int Width = imageHeader.biWidth, Height = imageHeader.biHeight;
			imageHeader.biWidth = Height;
			imageHeader.biHeight = Width;
		}

		public void Girar180Grados() {
			for(int y = 0; y < imageHeader.biHeight / 2; ++y) {
				for(int x = 0; x < imageHeader.biWidth; ++x) {
					uint pixel_1 = ReadPixel(imageData, x, y), pixel_2 = ReadPixel(imageData, (imageHeader.biWidth - 1) - x, (imageHeader.biHeight - 1) - y);
					WritePixel(imageData, x, y, pixel_2);
					WritePixel(imageData, (imageHeader.biWidth - 1) - x, (imageHeader.biHeight - 1) - y, pixel_1);
				}
			}
		}

		public void Girar90GradosDerecha() {
			int sizeOfNewImage = 0, bytesPerRow, bytesUsedPerRow, bytesPaddingPerRow, pixelsPerRow = imageHeader.biHeight;
			switch(imageHeader.biBitCount) {
				case 1:
					bytesUsedPerRow = (int)Math.Ceiling((double)pixelsPerRow / 8.0);
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					sizeOfNewImage = bytesPerRow * imageHeader.biWidth;
					break;
				case 4:
					bytesUsedPerRow = (int)Math.Ceiling((double)pixelsPerRow / 2.0);
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					sizeOfNewImage = bytesPerRow * imageHeader.biWidth;
					break;
				case 8:
					bytesUsedPerRow = pixelsPerRow;
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					sizeOfNewImage = bytesPerRow * imageHeader.biWidth;
					break;
				case 24:
					bytesUsedPerRow = pixelsPerRow * 3;
					bytesPaddingPerRow = ((bytesUsedPerRow % 4) == 0) ? 0 : 4 - (bytesUsedPerRow % 4);
					bytesPerRow = bytesUsedPerRow + bytesPaddingPerRow;
					sizeOfNewImage = bytesPerRow * imageHeader.biWidth;
					break;
			}
			byte[] newImageData = new byte[sizeOfNewImage];
			for(int y = 0; y < imageHeader.biHeight; ++y) {
				for(int x = 0; x < imageHeader.biWidth; ++x) {
					WritePixel(newImageData, (imageHeader.biHeight - 1) - y, x, ReadPixel(imageData, x, y), imageHeader.biHeight);
				}
			}
			imageData = newImageData;
			int Width = imageHeader.biWidth, Height = imageHeader.biHeight;
			imageHeader.biWidth = Height;
			imageHeader.biHeight = Width;
		}

	}
}
