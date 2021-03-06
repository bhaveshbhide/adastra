﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Accord.Statistics.Analysis;
using AForge.Neuro;
using AForge.Neuro.Learning;
using Eloquera.Client;
using Adastra.Algorithms;
using Encog.MathUtil.RBF;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.Neural.RBF;
using Encog.Util.Simple;
using Encog.Neural.Networks.Training.Propagation.Resilient;

namespace Adastra
{
    /// <summary>
    /// Manages computed models which are the result of traing algorithms
    /// </summary>
    public class ModelStorage
    {
        DB db;

        public ModelStorage()
        {
            db = new DB(DbSettings.ConnectionString);
        }

        public void SaveModel(AMLearning model)
        {
            if (!File.Exists(DbSettings.fullpath + ".eq"))
            {
                db.CreateDatabase(DbSettings.fullpath);
            }

            db.OpenDatabase(DbSettings.fullpath);
            db.RefreshMode = ObjectRefreshMode.AlwaysReturnUpdatedValues;

            db.Store(model);

            db.Close();
        }

        public List<AMLearning> LoadModels()
        {
            List<AMLearning> result=new List<AMLearning>();

            if (File.Exists(DbSettings.fullpath + ".eq"))
            {
                db.OpenDatabase(DbSettings.fullpath);

                db.RefreshMode = ObjectRefreshMode.AlwaysReturnUpdatedValues;

                if (db.IsTypeRegistered(typeof(AMLearning).FullName))
                {
                    var query = from AMLearning model in db select model;
                    result = query.ToList();
                }

                db.Close();
            }
            return result;
        }
    }
}
