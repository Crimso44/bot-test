using ChatBot.Admin.Common.Exceptions;

namespace ChatBot.Admin.Common.Optional
{
    public class Optional<T>
    {
        private readonly bool _hasValue;

        private readonly T _value;

        public Optional(T value)
        {
            _value = value;
            _hasValue = true;
        }

        public bool HasValue => _hasValue;

        public T Value
        {
            get
            {
                if (!_hasValue)
                {
                    throw new OptionalException("Optional has no value set");
                }


                return _value;
            }
        }

        public override bool Equals(object other)
        {
            if (!_hasValue)
            {
                return other == null;
            }

            if (other == null)
            {
                return false;
            }

            return _value.Equals(other);
        }

        public override int GetHashCode()
        {
            return _hasValue ? _value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return _hasValue ? _value.ToString() : string.Empty;
        }
    }
}
