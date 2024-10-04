namespace Mapper
{
    public class OneToManyMapper : IOneToManyMapper
    {
        private const int _minNumberAllowed = 1;
        private const int _maxNumberAllowed = 947483647;

        // hashset so that I dont need to handle duplicates and long lookup over it.
        private Dictionary<int, HashSet<int>> _mapper;

        public OneToManyMapper(Dictionary<int, HashSet<int>>? Mapper = null) // just for test
        {
            _mapper = Mapper ?? [];
        }

        public void Add(int parent, int child)
        {
            if (InvalidEntries(parent, child))
                throw new ArgumentException($"Parent and child parameter may be between 1 and {_maxNumberAllowed}");

            foreach (var keyValuePair in _mapper) // ok. we can have O(n) worst case for keys here
            {
                if (keyValuePair.Value.Contains(child) && keyValuePair.Key != parent) // O(1)
                    throw new ArgumentException($"This child has another parent");
            }

            if (_mapper.ContainsKey(parent))
            {
                _mapper[parent].Add(child);
                return;
            }

            _mapper.Add(parent, [child]);
        }

        public IEnumerable<int> GetChildren(int parent)
        {
            if (InvalidEntries(parent))
                throw new ArgumentException($"{nameof(parent)} parameter may be between 1 and {_maxNumberAllowed}");

            if (!_mapper.TryGetValue(parent, out _))
                return [];

            return _mapper[parent];
        }

        public int GetParent(int child)
        {
            if (InvalidEntries(child))
                throw new ArgumentException($"{nameof(child)} parameter may be between 1 and {_maxNumberAllowed}");

            foreach (var keyValuePair in _mapper) // ok. we can have log(n) worst case here
            {
                if (keyValuePair.Value.Contains(child)) // O(1) => value == hashset
                    return keyValuePair.Key;
            }

            return 0;
        }

        public void RemoveChild(int child)
        {
            if (InvalidEntries(child))
                throw new ArgumentException($"{nameof(child)} parameter may be between 1 and {_maxNumberAllowed}");

            foreach (var keyValuePair in _mapper) // ok. we can have log(n) worst case here
            {
                if (keyValuePair.Value.Contains(child)) // O(1) => value == hashset
                    keyValuePair.Value.Remove(child);
            }
        }

        public void RemoveParent(int parent)
        {
            if (InvalidEntries(parent))
                throw new ArgumentException($"{nameof(parent)} parameter may be between 1 and {_maxNumberAllowed}");

            var removed = _mapper.Remove(parent); // O(1) 

            if (!removed)
                throw new ArgumentException($"{nameof(parent)} doesn't exist");
        }

        public void UpdateChild(int oldChild, int newChild)
        {
            if (InvalidEntries(oldChild, newChild))
                throw new ArgumentException($"{nameof(oldChild)} or {nameof(newChild)} parameter may be between 1 and {_maxNumberAllowed}");

            foreach (var keyValuePair in _mapper) // ok. we can have O(n) worst case for keys here
            {
                if (keyValuePair.Value.Contains(oldChild)) // O(1) => value == hashset
                {
                    keyValuePair.Value.Remove(oldChild); // O(1) => value == hashset
                    keyValuePair.Value.Add(newChild); // O(1) => value == hashset
                }
            }
        }

        public void UpdateParent(int oldParent, int newParent)
        {
            if (InvalidEntries(oldParent, newParent))
                throw new ArgumentException($"{nameof(oldParent)} or {nameof(newParent)} parameter may be between 1 and {_maxNumberAllowed}");

            if (_mapper.ContainsKey(newParent))
            {
                foreach (var value in _mapper[oldParent])
                {
                    _mapper[newParent].Add(value);
                    ;
                }
                _mapper[oldParent] = [];

                return;
            }

            _mapper.Add(newParent, _mapper[oldParent]);
            _mapper[oldParent] = [];
        }

        private static bool InvalidEntries(params int[] nums)
        {
            foreach (int num in nums)
            {
                if (num < _minNumberAllowed || num > _maxNumberAllowed)
                    return true;
            }

            return false;
        }
    }
}
