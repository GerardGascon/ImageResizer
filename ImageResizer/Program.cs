using System.Drawing;

namespace ImageResizer {
	internal class Program {
		static Bitmap DuplicateImageWithTransparentPixels(Bitmap originalImage, int additionalWidth,
			int additionalHeight) {
			// Calculate the new width and height of the image
			int newWidth = originalImage.Width + additionalWidth;
			int newHeight = originalImage.Height + additionalHeight;

			// Create a new bitmap with the calculated width and height
			Bitmap newImage = new(newWidth, newHeight);

			// Get the Graphics object for the new image
			using Graphics g = Graphics.FromImage(newImage);
			// Set the background color of the new image to transparent
			g.Clear(Color.Transparent);

			// Draw the original image onto the new image
			g.DrawImage(originalImage, 0, 0);

			// Fill the additional width and height with transparent pixels
			Rectangle transparentRect =
				new(originalImage.Width, originalImage.Height, additionalWidth, additionalHeight);

			using Brush transparentBrush = new SolidBrush(Color.Transparent);
			g.FillRectangle(transparentBrush, transparentRect);

			return newImage;
		}

		static int CalculateNextMultipleOfFour(int number) {
			if (number % 4 == 0) {
				// If the number is already a multiple of 4, return the same number
				return number;
			}

			// Calculate the next multiple of 4
			int nextMultiple = (number / 4 + 1) * 4;
			return nextMultiple;
		}

		static void Main(string[] args) {
			Console.WriteLine("This process will override the images, do you wish to continue? (y/n)");
			while (true) {
				string? c = Console.ReadLine();
				switch (c) {
					case "y": {
						foreach (string s in args) {
							Image img = Image.FromFile(s);
							Size defaultSize = img.Size;
							Size additionalSize = new Size(
								CalculateNextMultipleOfFour(img.Width) - img.Width,
								CalculateNextMultipleOfFour(img.Height) - img.Height
							);
							
							Image newImg = DuplicateImageWithTransparentPixels(new Bitmap(img), additionalSize.Width, additionalSize.Height);
							//Remove usage reference to file
							img.Dispose();
							//Delete file
							File.Delete(s);
							
							newImg.Save(s);
							
							newImg.Dispose();
							Console.WriteLine(string.Join(" - ", Path.GetFileName(s),
								$"Changed its size from {defaultSize.Width}x{defaultSize.Height} to {defaultSize.Width + additionalSize.Width}x{defaultSize.Height + additionalSize.Height}"));
						}
			
						Console.WriteLine("Process completed, press any key to exit...");
						Console.ReadKey();
						return;
					}
					case "n":
						return;
				}
			}
		}
	}
}