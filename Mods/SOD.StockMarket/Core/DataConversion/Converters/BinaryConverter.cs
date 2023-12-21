﻿using SOD.Common.Shadows.Implementations;
using System;
using System.Collections.Generic;

namespace SOD.StockMarket.Core.DataConversion.Converters
{
    /// <summary>
    /// Converter to convert stock data into/from binary files.
    /// </summary>
    internal class BinaryConverter : IDataConverter
    {
        private BinaryConverter() { }

        /// <summary>
        /// Create's a new instance.
        /// </summary>
        /// <returns></returns>
        internal static BinaryConverter Create()
        {
            return new BinaryConverter();
        }

        /// <inheritdoc/>
        public List<StockDataIO.StockDataDTO> Load(string path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Save(List<StockDataIO.StockDataDTO> data, MersenneTwisterRandom random, string path)
        {
            throw new NotImplementedException();
        }
    }
}
