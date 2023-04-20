﻿using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Transactions
{
    /// <summary>
    /// Returns base fee per gas and transaction effective priority fee per gas history for the requested block range if available. The range between headBlock-4 and headBlock is guaranteed to be available while retrieving data from the pending block and older history are optional to support. For pre-EIP-1559 blocks the gas prices are returned as rewards and zeroes are returned for the base fee per gas.
    /// </summary>
    public class EthFeeHistory : RpcRequestResponseHandler<FeeHistoryResult>, IEthFeeHistory
    {
        public static bool UseBlockCountAsNumber { get; set; } = false;

        public EthFeeHistory(IClient client) : base(client, ApiMethods.eth_feeHistory.ToString())
        {
        }

        /// <summary>
        /// Returns base fee per gas and transaction effective priority fee per gas history for the requested block range if available. The range between headBlock-4 and headBlock is guaranteed to be available while retrieving data from the pending block and older history are optional to support. For pre-EIP-1559 blocks the gas prices are returned as rewards and zeroes are returned for the base fee per gas.
        /// </summary>
        /// <param name="blockCount">Number of blocks in the requested range. Between 1 and 1024 blocks can be requested in a single query. Less than requested may be returned if not all blocks are available.</param>
        /// <param name="highestBlockNumber">Highest number block of the requested range.</param>
        /// <param name="rewardPercentiles">A monotonically increasing list of percentile values to sample from each block's effective priority fees per gas in ascending order, weighted by gas used.
        /// Floating point value between 0 and 100.</param>
        /// <returns></returns>
        public Task<FeeHistoryResult> SendRequestAsync(HexBigInteger blockCount, BlockParameter highestBlockNumber, decimal[] rewardPercentiles = null, object id = null)
        {
            ValidateBlockCountRange(blockCount);
            if (highestBlockNumber == null) throw new ArgumentNullException(nameof(highestBlockNumber));
            ValidateRewardPercentiles(rewardPercentiles);
            
            if (!UseBlockCountAsNumber)
            {
                if (rewardPercentiles != null)
                {
                    return base.SendRequestAsync(id, blockCount, highestBlockNumber, rewardPercentiles);
                }
                else
                {
                    return base.SendRequestAsync(id, blockCount, highestBlockNumber, new double[] { });
                }
            }
            else
            {
                if (rewardPercentiles != null)
                {
                    return base.SendRequestAsync(id, blockCount.Value, highestBlockNumber, rewardPercentiles);
                }
                else
                {
                    return base.SendRequestAsync(id, blockCount.Value, highestBlockNumber, new double[] { });
                }
            }
        }

        private static void ValidateBlockCountRange(BigInteger blockCount)
        {
            if (blockCount < 1 || blockCount > 1024) throw new ArgumentOutOfRangeException(nameof(blockCount),
                "Reward percentiles need to be in a range of 1 - 1024, current value: " + blockCount);
        }

        private static void ValidateRewardPercentiles(decimal[] rewardPercentiles)
        {
            if (rewardPercentiles != null)
            {
                foreach (var rewardPercentile in rewardPercentiles)
                {
                    if (rewardPercentile < 0 || rewardPercentile > 100)
                        throw new ArgumentOutOfRangeException(nameof(rewardPercentiles),
                            "Reward percentiles need to be in a range of 0 - 100, current value: " + rewardPercentile);
                }
            }
        }

        /// <summary>
        /// Builds the Request, to return base fee per gas and transaction effective priority fee per gas history for the requested block range if available. The range between headBlock-4 and headBlock is guaranteed to be available while retrieving data from the pending block and older history are optional to support. For pre-EIP-1559 blocks the gas prices are returned as rewards and zeroes are returned for the base fee per gas.
        /// </summary>
        /// <param name="blockCount">Number of blocks in the requested range. Between 1 and 1024 blocks can be requested in a single query. Less than requested may be returned if not all blocks are available.</param>
        /// <param name="highestBlockNumber">Highest number block of the requested range.</param>
        /// <param name="rewardPercentiles">A monotonically increasing list of percentile values to sample from each block's effective priority fees per gas in ascending order, weighted by gas used.
        /// Floating point value between 0 and 100.</param>
        /// <returns></returns>
        public RpcRequest BuildRequest(HexBigInteger blockCount, BlockParameter highestBlockNumber, decimal[] rewardPercentiles = null, object id = null)
        {
            ValidateBlockCountRange(blockCount);
            if (highestBlockNumber == null) throw new ArgumentNullException(nameof(highestBlockNumber));
            ValidateRewardPercentiles(rewardPercentiles);

            if (!UseBlockCountAsNumber)
            {
                if (rewardPercentiles != null)
                {
                    return base.BuildRequest(id, blockCount, highestBlockNumber, rewardPercentiles);
                }
                else
                {
                    return base.BuildRequest(id, blockCount, highestBlockNumber, new double[] { });
                }
            }
            else
            {
                if (rewardPercentiles != null)
                {
                    return base.BuildRequest(id, blockCount.Value, highestBlockNumber, rewardPercentiles);
                }
                else
                {
                    return base.BuildRequest(id, blockCount.Value, highestBlockNumber, new double[] { });
                }
            }
           
        }
    }
}