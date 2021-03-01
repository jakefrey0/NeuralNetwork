/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 1/6/2021
 * Time: 6:43 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;

namespace NeuralNet {
	
	public static class Util {
		
		/// <summary>
		/// Convert an image to a list of bytes comprehensible by a neural network
		/// Supports black and white images
		/// </summary>
		/// <param name="fn">The file name</param>
		/// <returns>The list of bytes. 0-255 where 0 is black and 255 is white.</returns>
		public static IEnumerable<Byte> imageToNeuralData (String fn) { return Util.imageToNeuralData((Bitmap)(Bitmap.FromFile(fn))); }
		
		/// <summary>
		/// Convert an image to a list of bytes comprehensible by a neural network
		/// Supports black and white images
		/// </summary>
		/// <param name="img">The image</param>
		/// <returns>The list of bytes. 0-255 where 0 is black and 255 is white.</returns>
		public static IEnumerable<Byte> imageToNeuralData (Bitmap img) {
			
			UInt16 chk=(UInt16)(img.Size.Height*img.Size.Width);
			Byte x=0,y=0,chk0=(Byte)(img.Size.Width-1),chk1=(Byte)(img.Size.Height);
			while (y!=chk1) {
				
				yield return img.GetPixel(x,y).R;
				
				if (x==chk0) {
					
					x=0;
					++y;
					
				}
				else ++x;
				
			}
			
			img.Dispose();
			
		}
		
		//TODO:: maybe add a function to resize an image to the correct size while maintaining aspect ratio? it's easy, built in to the bitmap constructor.
		
		/// <summary>
		/// A slightly modified variation of the sigmoid mathematical function, which will take any given number and compress it to a Byte 0-FF.
		/// </summary>
		/// <param name="i">The aforementioned 'given number'</param>
		/// <returns>The compressed Byte result</returns>
		public static Byte sigmoid (this Single i) { return (Byte) ( ( (1)/(1+(Math.Exp((Double)(-i)))) ) * 255 ); }
		
		public static Single sigmoidF (this Single i) { return (i.sigmoid()/255F); }
		
		
		
	}
	
}