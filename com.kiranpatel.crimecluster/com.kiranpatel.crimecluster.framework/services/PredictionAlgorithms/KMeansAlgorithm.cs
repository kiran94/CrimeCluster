namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;
	using System.Collections.Generic;

	using Accord.MachineLearning;

	public class KMeansAlgorithm : IPredictionService
	{
		public KMeansAlgorithm()
		{
		}

		public ICollection<Incident> predict(ICollection<Incident> dataSet)
		{
			double[][] obs = dataSet.Select(x => new double[] { x.Location.Latitude.Value, x.Location.Longitude.Value }).ToArray(); 

			KMeans kmeans = new KMeans(14);
			KMeansClusterCollection collection = kmeans.Learn(obs);



			foreach (var col in collection.Clusters)
			{
				Console.WriteLine(String.Join(",", col.Covariance[0][0], col.Covariance[1][0])); 
			}


			throw new NotImplementedException();
		}
	}
}
