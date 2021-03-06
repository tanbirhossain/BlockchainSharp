﻿namespace BlockchainSharp.Tests.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using BlockchainSharp.Core;
    using BlockchainSharp.Core.Types;
    using BlockchainSharp.Tests.TestUtils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BlockChainTests
    {
        [TestMethod]
        public void AddInitialBlock()
        {
            Block block = new Block(0, null);
            BlockChain blockchain = FactoryHelper.CreateBlockChain();

            Assert.IsTrue(blockchain.TryToAdd(block));
            Assert.AreEqual(block.Number, blockchain.BestBlockNumber);
        }

        [TestMethod]
        public void RejectNonGenesisInitialBlock()
        {
            Block genesis = new Block(0, null);
            Block block = new Block(1, genesis.Hash);
            Assert.IsFalse(FactoryHelper.CreateBlockChain().TryToAdd(block));
        }

        [TestMethod]
        public void AddGenesisChildBlock()
        {
            Block genesis = new Block(0, null);
            Block block = new Block(1, genesis.Hash);

            BlockChain chain = FactoryHelper.CreateBlockChain();

            Assert.IsTrue(chain.TryToAdd(genesis));
            Assert.IsTrue(chain.TryToAdd(block));
            Assert.AreEqual(block.Number, chain.BestBlockNumber);
        }

        [TestMethod]
        public void AddChildBlock()
        {
            Block genesis = new Block(0, null);
            Block parent = new Block(1, genesis.Hash);
            Block block = new Block(2, parent.Hash);

            BlockChain chain = FactoryHelper.CreateBlockChain();

            Assert.IsTrue(chain.TryToAdd(genesis));
            Assert.IsTrue(chain.TryToAdd(parent));
            Assert.IsTrue(chain.TryToAdd(block));
            Assert.AreEqual(block.Number, chain.BestBlockNumber);
        }

        [TestMethod]
        public void RejectBlockWithDifferentParent()
        {
            Block genesis = new Block(0, null);
            Block block = new Block(1, new BlockHash(new byte[] { 1, 2, 3 }));

            BlockChain chain = FactoryHelper.CreateBlockChain();

            Assert.IsTrue(chain.TryToAdd(genesis));
            Assert.IsFalse(chain.TryToAdd(block));
            Assert.AreEqual(genesis.Number, chain.BestBlockNumber);
        }

        [TestMethod]
        public void RejectBlockWithInvalidNumber()
        {
            Block genesis = new Block(0, null);
            Block block = new Block(2, genesis.Hash);

            BlockChain chain = FactoryHelper.CreateBlockChain();

            Assert.IsTrue(chain.TryToAdd(genesis));

            Assert.IsFalse(chain.TryToAdd(block));
            Assert.AreEqual(genesis.Number, chain.BestBlockNumber);
        }

        [TestMethod]
        public void GetBlockByNumber()
        {
            IList<Block> blocks = new List<Block>();
            Block parent = null;
            BlockChain chain = FactoryHelper.CreateBlockChain();

            for (uint k = 0; k < 10; k++)
            {
                Block block = new Block(k, parent != null ? parent.Hash : null);
                blocks.Add(block);
                parent = block;

                Assert.IsTrue(chain.TryToAdd(block));
            }

            Assert.AreEqual(9ul, chain.BestBlockNumber);

            for (uint k = 0; k < 10; k++)
            {
                Block block = chain.GetBlock(k);

                Assert.IsNotNull(block);
                Assert.AreEqual(blocks[(int)k], block);
                Assert.AreEqual(k, block.Number);
            }

            Assert.IsNull(chain.GetBlock(10));
            Assert.IsNull(chain.GetBlock(20));
        }
    }
}

