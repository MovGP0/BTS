using MathNet.Numerics.LinearAlgebra.Single;
using NUnit.Framework;

namespace Bts.Tests
{
    [TestFixture]
    public static class ElectionExtensionsTests
    {
        public sealed class AverageOfAnswerValues
        {
            [Test]
            public void ShouldBeZeroIfEmpty()
            {
                var election = new Election(3);

                var value = new DenseVector(new[] { 0f, 0f, 0f });

                Assert.That(election.AverageOfAnswers(), Is.EqualTo(value));
            }

            public sealed class ShouldBeTheAverageTestCase
            {
                internal DenseVector[] Values;
                internal DenseVector Expected;
            }

            public static ShouldBeTheAverageTestCase[] ShouldBeTheAverageTestCaseSource = 
            {
                new ShouldBeTheAverageTestCase
                {
                    Values = new[]
                    {
                        new DenseVector(new[] {1f, 0f}),
                        new DenseVector(new[] {0f, 1f})
                    },
                    Expected = new DenseVector(new[] {0.5f, 0.5f})
                },
                new ShouldBeTheAverageTestCase
                {
                    Values = new[]
                    {
                        new DenseVector(new[] {0f, 0f, 1f}),
                        new DenseVector(new[] {0f, 0f, 1f})
                    },
                    Expected = new DenseVector(new[] {0f, 0f, 1f})
                }, 
                new ShouldBeTheAverageTestCase
                {
                    Values = new[]
                    {
                        new DenseVector(new[] {0f, 0f, 1f}),
                        new DenseVector(new[] {0f, 1f, 0f})
                    },
                    Expected = new DenseVector(new[] {0f, 0.5f, 0.5f})
                }, 
                new ShouldBeTheAverageTestCase
                {
                    Values = new[]
                    {
                        new DenseVector(new[] {1f, 0f, 0f}),
                        new DenseVector(new[] {0f, 1f, 0f}),
                        new DenseVector(new[] {1f, 0f, 0f}),
                        new DenseVector(new[] {0f, 0f, 1f})
                    },
                    Expected = new DenseVector(new[] {0.5f, 0.25f, 0.25f})
                }
            };

            [Test, TestCaseSource(nameof(ShouldBeTheAverageTestCaseSource))]
            public void ShouldBeTheAverage(ShouldBeTheAverageTestCase testCase)
            {
                var election = new Election(testCase.Values[0].Count);

                foreach (var value in testCase.Values)
                {
                    var answer = new Answer(value, value);
                    election.AddAnswerAndReturnVotingId(answer);
                }
                
                Assert.That(election.AverageOfAnswers(), Is.EqualTo(testCase.Expected));
            }
        }

        public sealed class FrequencyOfAnswerPredictions
        {
            [Test]
            public void ShouldReturnFrequency()
            {
                var election = new Election(3);
                
                var value1 = new DenseVector(new[] { 1f, 0f, 0f });
                var prediction1 = new DenseVector(new[] { 1f, 0f, 0f });
                var answer1 = new Answer(value1, prediction1);
                
                var value2 = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction2 = new DenseVector(new[] { 0f, 1f, 0f });
                var answer2 = new Answer(value2, prediction2);

                election.AddAnswerAndReturnVotingId(answer1);
                election.AddAnswerAndReturnVotingId(answer2);

                Assert.That(election.FrequencyOfAnswerPredictions(), Is.EqualTo(0f));
            }
        }
    }
}