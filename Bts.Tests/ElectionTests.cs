using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Single;
using NUnit.Framework;

namespace Bts.Tests
{
    [TestFixture]
    public sealed class ElectionTests
    {
        public sealed class ConstructorTests
        {
            [TestCase(1)]
            [TestCase(2)]
            [TestCase(3)]
            public void ShouldNotThrow(int value)
            {
                Assert.That(() => new Election(value), Throws.Nothing);
            }

            [TestCase(0)]
            [TestCase(-1)]
            [TestCase(-2)]
            public void ShouldThrowWhenArgumentIsZeroOrNegative(int value)
            {
                Assert.That(() => new Election(value), Throws.InstanceOf<ArgumentOutOfRangeException>());
            }
        }

        public sealed class NumberOfPossibleAnswersTests
        {
            [TestCase(1)]
            [TestCase(2)]
            [TestCase(3)]
            public void ShouldBeSetByConstructor(int value)
            {
                var election = new Election(value);
                Assert.That(election.NumberOfPossibleAnswers, Is.EqualTo(value));
            }
        }

        public sealed class AddAnswerAndReturnVotingIdTests
        {
            [Test]
            public void ShouldThrowArgumentNullException_WhenAnswerIsNull()
            {
                var election = new Election(3);

                Assert.That(() => election.AddAnswerAndReturnVotingId(null), Throws.InstanceOf<ArgumentNullException>());
            }

            [Test]
            public void ShouldThrowArgumentException_WhenNumberOfAnswerDifferFromNumberOfPossibleAnswers()
            {
                var election = new Election(3);
                var value = new DenseVector(new[] { 0f, 1f, 0f, 0f, 0f });
                var prediction = new DenseVector(new[] { 1f, 0f, 0f, 0f, 0f });
                var answer = new Answer(value, prediction);

                Assert.That(() => election.AddAnswerAndReturnVotingId(answer), Throws.InstanceOf<ArgumentException>());
            }

            [Test]
            public void ShouldNotThrowArgumentException_WhenNumberOfAnswerEqualsNumberOfPossibleAnswers()
            {
                var election = new Election(3);
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 1f, 0f, 0f });
                var answer = new Answer(value, prediction);

                Assert.That(() => election.AddAnswerAndReturnVotingId(answer), Throws.Nothing);
            }

            [Test]
            public void ShouldReturnVotingId()
            {
                var election = new Election(3);
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 1f, 0f, 0f });
                var answer = new Answer(value, prediction);

                var votingId = election.AddAnswerAndReturnVotingId(answer);

                Assert.That(votingId, Is.EqualTo(0));
            }

            [Test]
            public void VotingIdShouldIncreaseWithEachVoteAdded()
            {
                var election = new Election(3);
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 1f, 0f, 0f });
                var answer = new Answer(value, prediction);

                Assert.That(election.AddAnswerAndReturnVotingId(answer), Is.EqualTo(0));
                Assert.That(election.AddAnswerAndReturnVotingId(answer), Is.EqualTo(1));
                Assert.That(election.AddAnswerAndReturnVotingId(answer), Is.EqualTo(2));
            }
        }

        public sealed class AnserTest
        {
            [TestCase(1f,0f,0f)]
            [TestCase(0f,1f,0f)]
            [TestCase(0f,0f,1f)]
            [TestCase(0f,0.5f,0.5f)]
            public void ShouldReturnTheSameAnswer(float a, float b, float c)
            {
                var election = new Election(3);
                var value = new DenseVector(new[] { a, b, c });
                var prediction = new DenseVector(new[] { 1f, 0f, 0f });
                var answer = new Answer(value, prediction);

                var id = election.AddAnswerAndReturnVotingId(answer);

                Assert.That(election.Answer(id), Is.EqualTo(answer));
            }

            [Test]
            public void ShouldBeEqual()
            {
                var election = new Election(3);
                var value1 = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction1 = new DenseVector(new[] { 1f, 0f, 0f });
                var answer1 = new Answer(value1, prediction1);

                var value2 = new DenseVector(new[] { 1f, 0f, 0f });
                var prediction2 = new DenseVector(new[] { 0f, 1f, 0f });
                var answer2 = new Answer(value2, prediction2);

                election.AddAnswerAndReturnVotingId(answer1);
                var id2 = election.AddAnswerAndReturnVotingId(answer2);
                
                Assert.That(election.Answer(id2), Is.EqualTo(answer2));
            }

            [TestCase(-1)]
            [TestCase(0)]
            [TestCase(1)]
            [TestCase(2)]
            public void ShouldThrowArgumentOutOfRangeException(int index)
            {
                var election = new Election(3);
                Assert.That(() => election.Answer(index), Throws.InstanceOf<ArgumentOutOfRangeException>());
            }
        }

        public sealed class AnswersTests
        {
            [Test]
            public void ShouldBeEmpty_WhenNothingWasAdded()
            {
                var election = new Election(3);
                var query = election.Answers;

                Assert.That(query.Count(), Is.EqualTo(0));
            }

            [Test]
            public void ReturnAsManyAnswersAsWhereAdded()
            {
                var election = new Election(3);
                var query = election.Answers;

                var value1 = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction1 = new DenseVector(new[] { 1f, 0f, 0f });
                var answer = new Answer(value1, prediction1);
                
                election.AddAnswerAndReturnVotingId(answer);
                election.AddAnswerAndReturnVotingId(answer);

                Assert.That(query.Count(), Is.EqualTo(2));
            }

            [Test]
            public void ReturnTheAnswersThatWhereAdded()
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

                var answers = election.Answers.ToArray();
                Assert.That(answers[0], Is.EqualTo(answer1));
                Assert.That(answers[1], Is.EqualTo(answer2));
            }
        }
    }
}