using System;
using MathNet.Numerics.LinearAlgebra.Single;
using NUnit.Framework;

namespace Bts.Tests
{
    [TestFixture]
    public sealed class AnswerTests
    {
        public sealed class Constructor
        {
            [Test]
            public void ShouldThrowAnException_WhenValueIsNull()
            {
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                Assert.That(() => new Answer(null, prediction), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldThrowAnException_WhenPredictionIsNull()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                Assert.That(() => new Answer(value, null), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldNotThrowAnException()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                Assert.That(() => new Answer(value, prediction), Throws.Nothing);
            }

            [TestCase(0f, 1f, 1f)]
            [TestCase(0f, 1f, -1f)]
            [TestCase(0f, -1f, 0f)]
            [TestCase(0f, 0f, 0f)]
            [TestCase(-1f, 1f, -1f)]
            [TestCase(0f, 0.999f, 0f)]
            [TestCase(0f, 1.001f, 0f)]
            public void ShouldThrowAnException_WhenValueIsNotNormalized(float a, float b, float c)
            {
                var value = new DenseVector(new[] { a, b, c });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                Assert.That(() => new Answer(value, prediction), Throws.InstanceOf<ArgumentException>());
            }

            [TestCase(0f, 1f, 1f)]
            [TestCase(0f, 1f, -1f)]
            [TestCase(0f, -1f, 0f)]
            [TestCase(0f, 0f, 0f)]
            [TestCase(-1f, 1f, -1f)]
            [TestCase(0f, 0.999f, 0f)]
            [TestCase(0f, 1.001f, 0f)]
            public void ShouldThrowAnException_WhenPredictionIsNotNormalized(float a, float b, float c)
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { a, b, c });
                Assert.That(() => new Answer(value, prediction), Throws.InstanceOf<ArgumentException>());
            }

            [Test]
            public void ShouldThrowArgumentException_WhenValueAndPredictionHaveDifferentDimension()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f, 0f });
                Assert.That(() => new Answer(value, prediction), Throws.InstanceOf<ArgumentException>());
            }
        }

        public sealed class Value
        {
            [TestCase(1f, 0f, 0f)]
            [TestCase(0f, 1f, 0f)]
            [TestCase(0f, 0f, 1f)]
            [TestCase(0f, 0.5f, 0.5f)]
            public void ShouldReturnTheGivenValue(float a, float b, float c)
            {
                var value = new DenseVector(new[] { a, b, c });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                var answer = new Answer(value, prediction);

                Assert.That(answer.Value.Count, Is.EqualTo(3));
                Assert.That(answer.Value[0], Is.EqualTo(a));
                Assert.That(answer.Value[1], Is.EqualTo(b));
                Assert.That(answer.Value[2], Is.EqualTo(c));
            }
        }

        public sealed class Prediction
        {
            [TestCase(1f, 0f, 0f)]
            [TestCase(0f, 1f, 0f)]
            [TestCase(0f, 0f, 1f)]
            [TestCase(0f, 0.5f, 0.5f)]
            public void ShouldReturnTheGivenValue(float a, float b, float c)
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { a, b, c });
                var answer = new Answer(value, prediction);

                Assert.That(answer.Prediction.Count, Is.EqualTo(3));
                Assert.That(answer.Prediction[0], Is.EqualTo(a));
                Assert.That(answer.Prediction[1], Is.EqualTo(b));
                Assert.That(answer.Prediction[2], Is.EqualTo(c));
            }
        }

        public sealed class Dimension
        {
            [Test]
            public void ShouldReturnTheDimension_2()
            {
                var value = new DenseVector(new[] { 0f, 1f});
                var prediction = new DenseVector(new[] { 0f, 1f});
                var answer = new Answer(value, prediction);

                Assert.That(answer.Dimension, Is.EqualTo(2));
            }

            [Test]
            public void ShouldReturnTheDimension_3()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f});
                var answer = new Answer(value, prediction);

                Assert.That(answer.Dimension, Is.EqualTo(3));
            }
        }

        public sealed class EqualsTests
        {
            [Test]
            public void ShouldReturnTrue_WhenValuesAndPredictionsAreEqual()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                var answer1 = new Answer(value, prediction);
                var answer2 = new Answer(value, prediction);

                Assert.That(answer1, Is.EqualTo(answer2));
            }

            [Test]
            public void ShouldReturnFalse_WhenValuesAreNotEqual()
            {
                var value1 = new DenseVector(new[] { 0f, 1f, 0f });
                var value2 = new DenseVector(new[] { 1f, 0f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                var answer1 = new Answer(value1, prediction);
                var answer2 = new Answer(value2, prediction);

                Assert.That(answer1, Is.Not.EqualTo(answer2));
            }

            [Test]
            public void ShouldReturnFalse_WhenPredictionsAreNotEqual()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction1 = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction2 = new DenseVector(new[] { 1f, 0f, 0f });
                var answer1 = new Answer(value, prediction1);
                var answer2 = new Answer(value, prediction2);

                Assert.That(answer1, Is.Not.EqualTo(answer2));
            }

            [Test]
            public void ShouldReturnFalse_WhenAnswerIsNull()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                var answer = new Answer(value, prediction);
                
                Assert.That(answer.Equals(null), Is.False);
            }
        }

        public sealed class OperatorEqualTo
        {
            [Test]
            public void ShouldReturnFalse_WhenLeftIsNull()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                var answer = new Answer(value, prediction);

                Assert.That(answer == null, Is.False);
            }

            [Test]
            public void ShouldReturnFalse_WhenRightIsNull()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                var answer = new Answer(value, prediction);

                Assert.That(null == answer, Is.False);
            }

            [Test]
            public void ShouldReturnTrue_WhenBothAreNull()
            {
                Assert.That((Answer)null == (Answer)null, Is.True);
            }

            [Test]
            public void ShouldReturnTrue_WhenAnswersAreEqual()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                var answer1 = new Answer(value, prediction);
                var answer2 = new Answer(value, prediction);
                Assert.That(answer1 == answer2, Is.True);
            }
            
            [Test]
            public void ShouldReturnFalse_WhenValuesAreNotEqual()
            {
                var value1 = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                var answer1 = new Answer(value1, prediction);

                var value2 = new DenseVector(new[] { 1f, 0f, 0f });
                var answer2 = new Answer(value2, prediction);
                Assert.That(answer1 == answer2, Is.False);
            }
            
            [Test]
            public void ShouldReturnFalse_WhenPredictionsAreNotEqual()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction1 = new DenseVector(new[] { 1f, 0f, 0f });
                var answer1 = new Answer(value, prediction1);

                var prediction2 = new DenseVector(new[] { 0f, 1f, 0f });
                var answer2 = new Answer(value, prediction2);
                Assert.That(answer1 == answer2, Is.False);
            }
        }

        public sealed class OperatorNotEqualTo
        {
            [Test]
            public void ShouldReturnTrue_WhenLeftIsNull()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                var answer = new Answer(value, prediction);

                Assert.That(answer != null, Is.True);
            }

            [Test]
            public void ShouldReturnTrue_WhenRightIsNull()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                var answer = new Answer(value, prediction);

                Assert.That(null != answer, Is.True);
            }

            [Test]
            public void ShouldReturnFalse_WhenBothAreNull()
            {
                Assert.That((Answer)null != (Answer)null, Is.False);
            }

            [Test]
            public void ShouldReturnFalse_WhenAnswersAreEqual()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                var answer1 = new Answer(value, prediction);
                var answer2 = new Answer(value, prediction);
                Assert.That(answer1 != answer2, Is.False);
            }
            
            [Test]
            public void ShouldReturnTrue_WhenValuesAreNotEqual()
            {
                var value1 = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction = new DenseVector(new[] { 0f, 1f, 0f });
                var answer1 = new Answer(value1, prediction);

                var value2 = new DenseVector(new[] { 1f, 0f, 0f });
                var answer2 = new Answer(value2, prediction);
                Assert.That(answer1 != answer2, Is.True);
            }
            
            [Test]
            public void ShouldReturnTrue_WhenPredictionsAreNotEqual()
            {
                var value = new DenseVector(new[] { 0f, 1f, 0f });
                var prediction1 = new DenseVector(new[] { 0f, 1f, 0f });
                var answer1 = new Answer(value, prediction1);

                var prediction2 = new DenseVector(new[] { 1f, 0f, 0f });
                var answer2 = new Answer(value, prediction2);
                Assert.That(answer1 != answer2, Is.True);
            }
        }
    }
}