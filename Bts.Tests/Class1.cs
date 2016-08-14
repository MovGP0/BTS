using MathNet.Numerics.LinearAlgebra.Single;
using NUnit.Framework;

namespace Bts.Tests
{
    [TestFixture]
    public sealed class Foo
    {
        [Test]
        public void Bar()
        {
            const int numberOfPeople = 3;
            const int numbersOfPossibleAnswers = 3;
            var prediction = new DenseVector(new[] {0f, 1f, 0f});

            var answersAsVectorArray = new []
            {
                new Answer(new DenseVector(new [] {0f, 1f, 0f}), prediction),
                new Answer(new DenseVector(new [] {1f, 0f, 0f}), prediction), 
                new Answer(new DenseVector(new [] {1f, 0f, 0f}), prediction)
            };

            var answers = new DenseMatrix(numberOfPeople, numbersOfPossibleAnswers);

            for(var person = 0; person < numberOfPeople; person++)
            for(var answer = 0; answer < numbersOfPossibleAnswers; answer++)
            {
                answers[person, answer] = answersAsVectorArray[person].Value[answer];
            }
            
            Assert.That(answers[0,0], Is.EqualTo(0f));
            Assert.That(answers[0,1], Is.EqualTo(1f));
            Assert.That(answers[0,2], Is.EqualTo(0f));

            Assert.That(answers[1,0], Is.EqualTo(1f));
            Assert.That(answers[1,1], Is.EqualTo(0f));
            Assert.That(answers[1,2], Is.EqualTo(0f));

            Assert.That(answers[2,0], Is.EqualTo(1f));
            Assert.That(answers[2,1], Is.EqualTo(0f));
            Assert.That(answers[2,2], Is.EqualTo(0f));
        }
    }
}
