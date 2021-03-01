/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 2/26/2021
 * Time: 9:37 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace NeuralNet {
	
	public class Neuron {
		
		/// <summary>
		/// The value of the neuron
		/// </summary>
		public Byte activation;
		
		/// <summary>
		/// The 'offset' of the neuron
		/// </summary>
		public SByte bias;
		
		/// <summary>
		/// The multiplier to modify the bias and weights by when learning
		/// </summary>
		public Single toDescend;
		
	}
	
}