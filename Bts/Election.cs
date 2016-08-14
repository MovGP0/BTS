using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using NullGuard;

namespace Bts
{
    public static class ElectionExtensions
    {
        public static float FrequencyOfAnswerPredictions(this Election election)
        {
            return 0f;
        }

        public static DenseVector AverageOfAnswers(this Election election)
        {
            return !election.Answers.Any()
                ? new DenseVector( election.NumberOfPossibleAnswers )
                : AverageOf(ValuesOfAnswers(election.Answers));
        }

        private static DenseVector AverageOf(IEnumerable<DenseVector> vectors )
        {
            var sum = SumOfVectors(vectors);

            return sum/ vectors.Count();
        }

        private static DenseVector SumOfVectors(IEnumerable<DenseVector> vectors)
        {
            DenseVector result = null;

            foreach (var vector in vectors)
            {
                if (result == null)
                    result = new DenseVector(vector.Count);

                for (var index = 0; index < vector.Count; index++)
                {
                    result[index] += vector[index];
                }
            }

            return result;
        }



        private static IEnumerable<DenseVector> ValuesOfAnswers(IEnumerable<Answer> answers)
        {
            foreach (var answer in answers)
            {
                yield return answer.Value;
            }
        }
    }

    public sealed class Election
    {
        public int NumberOfPossibleAnswers { get; }
        private int _numberOfAnswers;
        private Matrix<float> Votes { [return:AllowNull]get; set; }
        private Matrix<float> Predictions { [return:AllowNull]get; set; }

        public Election(int numberOfPossibleAnswers)
        {
            NumberOfPossibleAnswers = numberOfPossibleAnswers;
        }

        private static DenseMatrix ToMatrix(DenseVector vector)
        {
            var matrix = new DenseMatrix(vector.Count, 1);

            for (var row = 0; row < vector.Count; row++)
            {
                matrix[row, 0] = vector[row];
            }

            return matrix;
        }

        public int AddAnswerAndReturnVotingId(Answer answer)
        {
            if(answer.Dimension != NumberOfPossibleAnswers)
                throw new ArgumentException();

            Votes = NewOrAppendMatrix(answer.Value, Votes);
            Predictions = NewOrAppendMatrix(answer.Prediction, Predictions);
            
            return Interlocked.Increment(ref _numberOfAnswers) - 1;
        }
        
        private Matrix<float> NewOrAppendMatrix(DenseVector vector, [AllowNull]Matrix<float> matrix)
        {
            return matrix == null 
                ? ToMatrix(vector) 
                : matrix.Append(ToMatrix(vector));
        }

        public IEnumerable<Answer> Answers
        {
            get
            {
                for (var index = 0; index < _numberOfAnswers; index++)
                    yield return Answer(index);
            }
        }

        private static DenseVector GetColumnAsVector(int column, Matrix<float> matrix)
        {
            var values = matrix.Column(column).ToArray();
            return new DenseVector(values); 
        }

        public Answer Answer(int votingId)
        {
            if (votingId < 0 || votingId >= _numberOfAnswers)
                throw new ArgumentOutOfRangeException(nameof(votingId));

            var votes = GetColumnAsVector(votingId, Votes);
            var predictions = GetColumnAsVector(votingId, Predictions);
            
            return new Answer(votes, predictions);
        }
    }
}