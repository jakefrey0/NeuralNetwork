/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 1/6/2021
 * Time: 6:35 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace NeuralNet {
	
	public class Program {
		
		/*
		private const Byte loopCtrMax=3;
		private const Single learnRateIncrement=0.00001F;
//		private static UInt16 hlSize=64;
		private static Single learnRate=0.00033F;
		private static Dictionary<Double,List<Double>> times=new Dictionary<Double,List<Double>>(1000);
		private static Byte loopCtr=Program.loopCtrMax;
		private List<Double> currTimes=new List<Double>();
		*/
		
		/// <summary>
		/// Machine learning attempt 3
		/// </summary>
		public static void Main (String[] args) {
			
			DateTime start0=DateTime.UtcNow;
			
			NeuralNetwork nn=new NeuralNetwork(new UInt16[]{1089,17,17,4})/*{descent=Program.learnRate}*/;
			
			DateTime start;
			TimeSpan ts;
			List<Byte> answers;
			String[] dirs=Directory.GetDirectories(@".\Dataset\"),files;
			Byte[] desiredAnswer;
			Random r=new Random();
			String dir,file;
			UInt32 correct=0,incorrect=0;
			Boolean step=false,vis=false;
			
			/*
			foreach (Byte @byte in
				nn.makePrediction(Util.imageToNeuralData(@"C:\Users\Elite\Documents\SharpDevelop Projects\NeuralNet\NeuralNet\bin\Debug\Dataset\0\number-1.png"),
			                  	new Byte[]{255,0,0,0,0,0,0,0,0,0},
			                  	false)
			        )
				Console.WriteLine(@byte.ToString()+',');
				
			goto halt;
			*/
			
			trainSubRt:
			
			while (!(Console.KeyAvailable&&Console.ReadKey(true).Key==ConsoleKey.Escape)) {
				
				start=DateTime.UtcNow;
				
				desiredAnswer=new Byte[]{0,0,0,0,0,0,0,0,0,0};
				dir=(dirs[r.Next(0,4)]);
				files=Directory.GetFiles(dir);
				file=files[r.Next(0,files.Length)];
				
				
				desiredAnswer[Byte.Parse(dir.Last().ToString())]=255;
				
//				desiredAnswer=new Byte[]{255,0,0,0,0,0,0,0,0,0};
//				file=@"C:\Users\Elite\Documents\SharpDevelop Projects\NeuralNet\NeuralNet\bin\Debug\Dataset\0\0.png";
				answers=nn.makePrediction(Util.imageToNeuralData(file),desiredAnswer,true,vis,delegate(){Program.Main(args);});
				
//				foreach (Byte @byte in answers)
//					Console.WriteLine(@byte.ToString()+',');
				
				ts=DateTime.UtcNow-start;
				
				Byte prediction=(Byte)(answers.IndexOf(answers.Max())),desiredPrediction=(Byte)(desiredAnswer.ToList().IndexOf(desiredAnswer.Max()));
				if (prediction==desiredPrediction) ++correct;
				else ++incorrect;
				Console.WriteLine("Answer: "+prediction.ToString()+", desired: "+desiredPrediction.ToString()+", R/W: "+correct.ToString()+'/'+incorrect.ToString()+"("+ts.TotalMilliseconds.ToString()+"ms, "+file+')');
				
				/*
				if (correct>incorrect||((correct+incorrect)>13000)) {
					
					Console.WriteLine("learnRate:"+Program.learnRate.ToString());
					
					if (Program.learnRate==0.06F) {
						
						List<String> lines=new List<String>();
						foreach (KeyValuePair<Double,List<Double>> kvp in Program.times) {
							String str="HL size: "+kvp.Key.ToString()+",Time (sec): ";
							foreach (Double dbl in kvp.Value)
								str+=dbl.ToString()+',';
							lines.Add(str);
						}
						
						File.WriteAllLines("./results",lines);
						return;
						
					}
					
					ts=DateTime.UtcNow-start0;
					if (Program.loopCtr==Program.loopCtrMax)
						Program.times.Add(Program.learnRate,new List<Double>(Program.loopCtrMax));
					
					times[Program.learnRate].Add(ts.TotalSeconds);
					
					if (Program.loopCtr==0) {
					
						Program.learnRate+=Program.learnRateIncrement;
						Program.loopCtr=Program.loopCtrMax;
						Program.Main(args);
						return;
					
					}
					else --Program.loopCtr;
					
				}*/
				if (step&&Console.ReadKey().Key==ConsoleKey.Escape) break;
				
			}
			
			
			while (true) {
			
				String str=Console.ReadLine();
				if (str=="continue"||str=="train") goto trainSubRt;
				else if (str=="break"||str=="stop") goto halt;
				else if (str=="visual"||str=="visualize") {
					vis=!vis;
					goto trainSubRt;
				}
				else if (str=="step") {
					step=!step;
					goto trainSubRt;
				}
				else {
					
					List<Byte> results
						=nn.makePrediction(Util.imageToNeuralData(str),
	                  	null,
	                  	false,
	                  	vis).ToList();
					
					foreach (Byte @byte in results)
						Console.Write(@byte.ToString()+',');
					
					Console.WriteLine("Your number was a: "+results.IndexOf(results.Max()).ToString());
					
				}
				
			}
			
			
			halt:
				goto halt;
		
		}
		
	}
	
}