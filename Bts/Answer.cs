using System;
using MathNet.Numerics.LinearAlgebra.Single;
using NullGuard;

namespace Bts
{
    public sealed class Answer : IEquatable<Answer>
    {
        public DenseVector Value { get; }
        public DenseVector Prediction { get; }
        public int Dimension => Value.Count;

        public Answer(DenseVector value, DenseVector prediction)
        {
            if(!IsNormalized(value))
                throw new ArgumentException(nameof(value), AnswerMessages.VectorWasNotNormalized);
            if(!IsNormalized(prediction))
                throw new ArgumentException(nameof(prediction), AnswerMessages.VectorWasNotNormalized);
            if(value.Count != prediction.Count)
                throw new ArgumentException(nameof(prediction), AnswerMessages.DifferentDimension);

            Value = value;
            Prediction = prediction;
        }

        public bool Equals([AllowNull]Answer other)
        {
            if(ReferenceEquals(null, other))
                return false;
            if(ReferenceEquals(this, other))
                return true;
            return Equals(Value, other.Value)
                && Equals(Prediction, other.Prediction);
        }

        private static bool IsNormalized(DenseVector vector)
        {
            return Math.Abs(vector.Sum() - 1d) < 0.000001;
        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(null, obj))
                return false;
            if(ReferenceEquals(this, obj))
                return true;
            return obj is Answer && Equals((Answer)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Value?.GetHashCode() ?? 0) * 397)
                    ^ (Prediction?.GetHashCode() ?? 0);
            }
        }

        public static bool operator ==([AllowNull]Answer left, [AllowNull]Answer right)
        {
            if(ReferenceEquals(left, right))
                return true;

            if(ReferenceEquals(null, left) || ReferenceEquals(null, right))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=([AllowNull]Answer left, [AllowNull]Answer right)
        {
            return !(left == right);
        }
    }
}