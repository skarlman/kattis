using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace KnightsInAFen.Tests
{
    [TestFixture]
    public class Tests
    {

        [TestCase("00100,10110,01 11,10111,01001", 2, 2)]
        [TestCase(" 0100,10110,00011,10111,01001", 0, 0)]
        [TestCase("10100,10110,00011,10111,0101 ", 4, 4)]
        [TestCase("10100,101 0,00011,10111,01011", 1, 3)]
        public void FindEmptySquare(string boardString, int expectedRow, int expectedColumn)
        {
            var board = boardString.Split(',').ToList();

            var result = KnightsInAFen.Program.FindEmptySquare(board);

            Assert.AreEqual(expectedRow, result.Row);
            Assert.AreEqual(expectedColumn, result.Column);
        }


        [Test]
        public void CanDetectCompleteBoard()
        {
            var board = new List<string>()
            {
                "11111","01111","00 11","00001","00000"
            };

            var result = KnightsInAFen.Program.NumberOfMisplacedPieces(board);

            Assert.AreEqual(0,result);
        }

        [Test]
        public void CanDetectInCompleteBoard()
        {
            var board = new List<string>()
            {
                "11111","10111","00 11","00001","00000"
            };

            var result = KnightsInAFen.Program.NumberOfMisplacedPieces(board);

            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetAllPossibleMoves()
        {
            //var board = new List<string>() {"00100",
            //                                "10110",
            //                                "01 11",
            //                                "10111",
            //                                "01001" };

            List<KnightsInAFen.Program.Position> result =
                KnightsInAFen.Program.GetAllPossibleMoves(new KnightsInAFen.Program.Position(2, 2));

            List<KnightsInAFen.Program.Position> expected = new List<KnightsInAFen.Program.Position>()
            {
                new KnightsInAFen.Program.Position(0, 1),
                new KnightsInAFen.Program.Position(0, 3),
                new KnightsInAFen.Program.Position(1, 0),
                new KnightsInAFen.Program.Position(1, 4),
                new KnightsInAFen.Program.Position(3, 0),
                new KnightsInAFen.Program.Position(3, 4),
                new KnightsInAFen.Program.Position(4, 1),
                new KnightsInAFen.Program.Position(4, 3)
            };

            CollectionAssert.AllItemsAreUnique(result);
            Assert.AreEqual(expected.Count, result.Count);
            foreach (var position in result)
            {
                Assert.IsTrue(expected.Any(e => e.Row == position.Row && e.Column == position.Column));
            }
            //CollectionAssert.AreEquivalent(expected,result);
        }


        [Test]
        public void GivenTestcases()
        {
            var input = @"01011
110 1
01110
01010
00100
10110
01 11
10111
01001
00000".Split(Environment.NewLine.ToCharArray()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            var result = KnightsInAFen.Program.CalculateMoves(input);

            Assert.AreEqual(11, result[0]);
            Assert.AreEqual(7, result[1]);
            //Assert.AreEqual("Unsolvable in less than 11 move(s).", result[0]);
            //Assert.AreEqual("Solvable in 7 move(s).", result[1]);
        }
    }
}
