﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public delegate void ExperimentCompletedEventHandler(int successRate);

    public class Experiment
    {
        EEGRecord _er; 
        AMLearning _ml;

        public Experiment(string name, EEGRecord er, AMLearning ml)
        {
			Name = name;
            _er = er;
            _ml = ml;

            //_ml.Progress += new ChangedValuesEventHandler(_ml_Progress);
        }

        public string Name { get; set; }

		public int Progress;

		//void _ml_Progress(int progress)
		//{
		//    if (this.Progress!=null)
		//    Progress(progress);
		//}

        public AMLearning Start()
        {
            //read ALL feature vectors
 
            //seperate data for train, evaluate and train
            _ml.Train(_er.FeatureVectorsInputOutput, _er.FeatureVectorsInputOutput[0].Length - 1);

            //supply to ml for train using train and evaluate

            //use test to produce results 
            //int result=100;

			//if (Completed != null)
			//    Completed(result);

			//this.Progress = 100;

            return _ml;
        }

		//public virtual event ChangedValuesEventHandler Progress;

		//public virtual event ExperimentCompletedEventHandler Completed;

		public void SetRecord(EEGRecord record)
		{
			_er = record;
		}
    }
}