/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 2/28/2021
 * Time: 10:23 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Drawing;

namespace NeuralNet {
	
	public static class Visualization {
		
		[DllImport("user32.dll")]
		private static extern IntPtr GetDC (IntPtr hWnd);
		
		[DllImport("user32.dll")]
		private static extern IntPtr ReleaseDC (IntPtr hWnd,IntPtr hDC);
		
		private const Byte maxLayerLengthToDraw=16,nrnSize=24;
		private const Int32 width=700,height=1000;
		private static Font font=new Font("Verdana",(Visualization.nrnSize/3),FontStyle.Italic);
		
		public static void drawNeurons (List<List<Neuron>> layers) {
			
			UInt16 x=(Visualization.nrnSize*2),y=x;
			List<List<Byte>> adjustedLayers=new List<List<Byte>>();
			
			IntPtr dc=Visualization.GetDC(IntPtr.Zero);
			
			using (Pen pen=new Pen(Color.Black))
			using (Pen pen0=new Pen(Color.White))
			using (Graphics g=Graphics.FromHdc(dc)) {
				
				g.FillRectangle(pen0.Brush,0,0,Visualization.width,Visualization.height);
				
				foreach (List<Neuron> layer in layers) {
					
					if (layer.Count<=Visualization.maxLayerLengthToDraw) {
						adjustedLayers.Add(layer.Select(x0=>x0.activation).ToList());
						continue;
					}
					
					UInt16 split=(UInt16)(layer.Count/Visualization.maxLayerLengthToDraw),j=0,ctr=0;
					Byte[] newNeurons=new Byte[Visualization.maxLayerLengthToDraw];
					
					Byte i=0;
					UInt32 sum=0;
					
					while (i<Visualization.maxLayerLengthToDraw) {
						
						while (j<split) {
							
							sum+=layer[ctr].activation;
							++j;
							++ctr;
							
						}
						
						newNeurons[i]=(Byte)(sum/j);
						sum=0;
						j=0;
						++i;
						
					}
					
					adjustedLayers.Add(newNeurons.ToList());
					
				}
				
				foreach (List<Byte> layer in adjustedLayers) {
					
					foreach (Byte b in layer) {
						
						g.DrawEllipse(pen,new RectangleF(x,y,Visualization.nrnSize,Visualization.nrnSize));
						g.DrawString(b.ToString(),Visualization.font,pen.Brush,x,y+(Visualization.nrnSize/4));
						y+=(Visualization.nrnSize*2);
						
					}
					
					x+=(Visualization.nrnSize*2);
					y=(Visualization.nrnSize*2);
					
				}
				
			}
			
			Visualization.ReleaseDC(IntPtr.Zero,dc);
			
		}
		
	}
	
}