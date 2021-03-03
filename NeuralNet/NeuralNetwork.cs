/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 2/26/2021
 * Time: 9:30 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNet {
	
	public class NeuralNetwork {
		
		/// <summary>
		/// The amount of neurons per layer
		/// </summary>
		private readonly UInt16[] layerData;
		
		/// <summary>
		/// The number of layers
		/// </summary>
		private readonly Byte layerCount;
		
		/// <summary>
		/// The global NeuralNetwork randomizer
		/// </summary>
		private static Random rdm=new Random();
		
		/// <summary>
		/// The layers
		/// </summary>
		private List<List<Neuron>> layers;
		
		/// <summary>
		/// The global NeuralNetwork learning rate
		/// </summary>
		private const Single descent=0.00033F;
		
		/// <summary>
		/// The connections between neurons
		/// </summary>
		private Dictionary<Neuron,Dictionary<Neuron,Single>> weights;
		
		public NeuralNetwork (UInt16[] layerLengths) {
			
			this.layerData=layerLengths;
			this.layerCount=(Byte)(this.layerData.Length);
			this.layers=new List<List<Neuron>>(this.layerCount);
			this.weights=new Dictionary<Neuron,Dictionary<Neuron,Single>>();
			
			Byte i=0;
			UInt16 j=0;
			while (i<this.layerCount) {
				
				this.layers.Add(new List<Neuron>(this.layerData[i]));
				while (j<layerData[i]) {
					
					this.layers[i].Add(new Neuron(){activation=0,bias=/*(SByte)(NeuralNetwork.rdm.Next(-14,15))*/0});
					++j;
					
				}
				
				++i;
				j=0;
				
			}
			
			while (j<(layerCount-1)) {
				
				foreach (Neuron n in this.layers[j]) {
					
					this.weights.Add(n,new Dictionary<Neuron,Single>(this.layerData[j+1]));
					foreach (Neuron n0 in this.layers[j+1])
						this.weights[n].Add(n0,(Single)((NeuralNetwork.rdm.NextDouble()-NeuralNetwork.rdm.NextDouble())/10F));
					
				}
				
				++j;
				
			}
			
		}
		
		/// <summary>
		/// Make a prediction
		/// </summary>
		/// <param name="input"></param>
		/// <param name="desired"></param>
		/// <param name="learn"></param>
		/// <returns></returns>
		public List<Byte> makePrediction (IEnumerable<Byte> input,Byte[] desired,Boolean learn,Boolean visualize=false,Action onFail=null) {
			
			//Clear all potential old neuron values
			foreach (List<Neuron> layer in this.layers.Skip(1))
				foreach (Neuron nrn in layer)
					nrn.activation=0;
			
			//Restructure input
			List<Byte> _input;
			if (input.GetType()==typeof(List<Byte>))
				_input=(List<Byte>)input;
			else _input=new List<Byte>(input);
			
			//Fill input layers
			UInt16 i=0;
			while (i<this.layerData[0]) /* possible data loss if input.Length>layerData[0] */ {
				
				this.layers[0][i].activation=_input[i];
				++i;
				
			}
			i=0;
			Single sum=0;
			
			while (i<(this.layerCount-1)) {
				
				foreach (Neuron neuron in this.layers[i+1]) {
					
					foreach (Neuron neuron0 in this.layers[i]) {
//						Console.WriteLine("Activation:"+neuron0.activation.ToString()+",bias:"+neuron0.bias.ToString()+",weight:"+this.weights[neuron0][neuron].ToString());
						sum+=(neuron0.activation+neuron0.bias)*(this.weights[neuron0][neuron])*0.5F;
					}
					
					neuron.activation=sum.sigmoid();
//					Console.WriteLine("Sum:"+sum.ToString()+",Sigmoided:"+sum.sigmoid().ToString());
					sum=0;
					
				}
				
				++i;
				
			}
			
			if (visualize) Visualization.drawNeurons(this.layers);
			if (learn) this.backPropogate(desired,onFail);
			
			return this.layers[this.layerCount-1].Select(x=>x.activation).ToList();
			
		}
		
		
		private void backPropogate (Byte[] desiredAnswers,Action onFail) {
			
			foreach (List<Neuron> layer in this.layers)
				foreach (Neuron n in layer)
					n.toDescend=0;
			
			foreach (Neuron n in this.layers.Last())
				n.toDescend=(n.activation-desiredAnswers[this.layers.Last().IndexOf(n)]);
			
			Byte i=(Byte)(this.layerCount-2);
			Single sum=0F;
			while (true) {
				
				foreach (Neuron n in this.layers[i]) {
					
					foreach (Neuron n0 in this.layers[i+1])
						sum+=(this.weights[n][n0]*n0.toDescend);
					
					n.toDescend=sum*((Single)(n.activation*n.bias)).sigmoidF();
//					Console.WriteLine("toDescend:"+n.toDescend.ToString()+",sum:"+sum.ToString()+",activation:"+n.activation.ToString()+",bias:"+n.bias.ToString());
					try { n.bias-=(SByte)(NeuralNetwork.descent*(n.toDescend)); }//TODO:: check if this can be better calculated (more specific?)
					catch { 
						
						if (onFail!=null)
							onFail.Invoke();
//						Console.WriteLine("toDescend:"+n.toDescend.ToString()+",sum:"+sum.ToString()+",activation:"+n.activation.ToString()+",bias:"+n.bias.ToString()); 
						
						
					}
					
					
					sum=0F;
					
				}
				
				if (i==0) break;
				--i;
				
			}
			
			i=0;
			while (i!=(this.layerCount-1)) {
				
				foreach (Neuron n in this.layers[i]) {
					
					foreach (Neuron n0 in this.layers[i+1]) {
						
//						Single prev=this.weights[n][n0];
						this.weights[n][n0]-=(NeuralNetwork.descent*(n.activation/255)*n0.toDescend);
//						Console.WriteLine("Old:"+prev.ToString()+",New:"+this.weights[n][n0].ToString()+"descent:"+NeuralNetwork.descent+",activation:"+n.activation+",toDescend:"+n0.toDescend);
						
					}
					
					
				}
				
				++i;
				
			}
			
		}
		
	}
	
}
