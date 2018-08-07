﻿using NBitcoin.Networks;
using Stratis.Bitcoin.Builder;
using Stratis.Bitcoin.Configuration;
using Stratis.Bitcoin.Features.BlockStore;
using Stratis.Bitcoin.Features.MemoryPool;
using Stratis.Bitcoin.Features.Miner;
using Stratis.Bitcoin.Features.RPC;
using Stratis.Bitcoin.Features.SmartContracts;
using Stratis.Bitcoin.Features.SmartContracts.Networks;
using Stratis.Bitcoin.Features.SmartContracts.Wallet;

namespace Stratis.Bitcoin.IntegrationTests.Common.Runners
{
    public sealed class StratisSmartContractPosNode : NodeRunner
    {
        public StratisSmartContractPosNode(string dataDir)
            : base(dataDir)
        {
            this.Network = NetworkRegistration.Register(new SmartContractPosRegTest());
        }

        public override void BuildNode()
        {
            var settings = new NodeSettings(this.Network, args: new string[] { "-conf=stratis.conf", "-datadir=" + this.DataFolder });

            this.FullNode = (FullNode)new FullNodeBuilder()
                .UseNodeSettings(settings)
                .UseBlockStore()
                .UseMempool()
                .AddRPC()
                    .AddSmartContracts()
                    .UseSmartContractPosConsensus()
                    .UseSmartContractWallet()
                    .UseSmartContractPosPowMining()
                    .UseReflectionExecutor()
                .MockIBD()
                .SubstituteDateTimeProviderFor<MiningFeature>()
                .Build();
        }

        public override void OnStart()
        {
            this.FullNode.Start();
        }
    }
}