using Mapper;

namespace Tests
{
    public class OneToManyMapperTests
    {
        private const int _maxNumberAllowed = 947483647;

        [Fact]
        public void OneToManyMapper_Add_ShouldDoesTheOperationCorrectly_WhenTheParentDoesntExist()
        {
            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10, 11 } },
                { 2, new HashSet<int> { 20, 21 } },
            };
            var oneToManyMapper = new OneToManyMapper(initialSample);

            oneToManyMapper.Add(3, 30);

            Assert.Equal(3, initialSample.Values.Count);
        }

        [Fact]
        public void OneToManyMapper_Add_ShouldDoesTheOperationCorrectly_WhenTheParentExists()
        {
            var parentThatAlreadyExists = 2;
            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10, 11 } },
                { parentThatAlreadyExists, new HashSet<int> { 20 } },
            };
            var oneToManyMapper = new OneToManyMapper(initialSample);

            oneToManyMapper.Add(parentThatAlreadyExists, 30);

            Assert.Equal(2, initialSample.Values.Count);
        }

        [Fact]
        public void OneToManyMapper_Add_ShouldDoesNotDuplicateEntries_WhenTheChildAlreadyExistsBellowTheSameParent()
        {
            var repeatedParentEntry = 2;
            var repeatedChildEntry = 20;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10, 11 } },
                { repeatedParentEntry, new HashSet<int> { repeatedChildEntry } },
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            oneToManyMapper.Add(repeatedParentEntry, repeatedChildEntry);

            Assert.Equal(2, initialSample.Values.Count);
            Assert.Single(initialSample[repeatedParentEntry]);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData((_maxNumberAllowed + 1), 1)]
        [InlineData(1, (_maxNumberAllowed + 1))]
        public void OneToManyMapper_Add_ShouldThrowsAnException_WhenNotValidEntries(int parent, int child)
        {
            var oneToManyMapper = new OneToManyMapper();

            Assert.Throws<ArgumentException>(() => oneToManyMapper.Add(parent, child));
        }

        [Fact]
        public void OneToManyMapper_Add_ShouldThrowsAnException_WhenChildHasAnotherParent()
        {
            var childThatHasAnotherParent = 20;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10 } },
                { 2, new HashSet<int> { childThatHasAnotherParent } },
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            Assert.Throws<ArgumentException>(() => oneToManyMapper.Add(3, childThatHasAnotherParent));
        }

        [Fact]
        public void OneToManyMapper_GetChildren_ShouldReturnsEmptyList_WhenParentDoesNotExist()
        {
            var parentThatDoesNotExist = 3;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10 } },
                { 2, new HashSet<int> { 20 } },
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            var result = oneToManyMapper.GetChildren(parentThatDoesNotExist);

            Assert.Empty(result);
        }

        [Fact]
        public void OneToManyMapper_GetChildren_ShouldReturnsChildrenCorrectly()
        {
            var parent = 1;
            var expectedChilden = new HashSet<int> { 10, 11, 12 };

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { parent, expectedChilden },
                { 2, new HashSet<int> { 20 } },
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            var result = oneToManyMapper.GetChildren(parent);

            Assert.Equal(expectedChilden, result);
        }


        [Theory]
        [InlineData(-1)]
        [InlineData((_maxNumberAllowed + 1))]
        public void OneToManyMapper_GetChildren_ShouldThrowsException_WhenNotValidEntry(int parent)
        {
            var oneToManyMapper = new OneToManyMapper();

            Assert.Throws<ArgumentException>(() => oneToManyMapper.GetChildren(parent));
        }

        [Fact]
        public void OneToManyMapper_GetParent_ShouldReturnsZero_WhenParentDoesNotExist()
        {
            var childrenThatDoesNotExist = 30;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10 } },
                { 2, new HashSet<int> { 20 } },
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            var result = oneToManyMapper.GetParent(childrenThatDoesNotExist);

            Assert.Equal(0, result);
        }

        [Fact]
        public void OneToManyMapper_GetParent_ShouldReturnsParent_WhenParentExists()
        {
            var childrenExists = 20;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10 } },
                { 2, new HashSet<int> { childrenExists } },
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            var result = oneToManyMapper.GetParent(childrenExists);

            Assert.Equal(2, result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData((_maxNumberAllowed + 1))]
        public void OneToManyMapper_GetParent_ShouldThrowsException_WhenNotValidEntry(int child)
        {
            var oneToManyMapper = new OneToManyMapper();

            Assert.Throws<ArgumentException>(() => oneToManyMapper.GetParent(child));
        }

        [Fact]
        public void OneToManyMapper_RemoveChild_ShouldRemovesChild_WhenChildExists()
        {
            var childToRemove = 21;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10 } },
                { 2, new HashSet<int> { 20, childToRemove } },
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            oneToManyMapper.RemoveChild(childToRemove);

            Assert.Equal(initialSample[2], [20]);
        }

        [Fact]
        public void OneToManyMapper_RemoveChild_ShouldDoesNothing_WhenChildDoesNotExist()
        {
            var childDoesNotExist = 22;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10 } },
                { 2, new HashSet<int> { 20, 21 } },
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            oneToManyMapper.RemoveChild(childDoesNotExist);

            Assert.Equal(initialSample[2], [20, 21]);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData((_maxNumberAllowed + 1))]
        public void OneToManyMapper_RemoveChild_ShouldThrowsException_WhenNotValidEntry(int child)
        {
            var oneToManyMapper = new OneToManyMapper();

            Assert.Throws<ArgumentException>(() => oneToManyMapper.RemoveChild(child));
        }

        [Fact]
        public void OneToManyMapper_RemoveParent_ShouldRemovesParent_WhenParentExists()
        {
            var parentToRemove = 3;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10, 11 } },
                { 2, new HashSet<int> { 20, 21 } },
                { parentToRemove, new HashSet<int> { 30, 31 } }
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            oneToManyMapper.RemoveParent(parentToRemove);

            Assert.Equal(2, initialSample.Count);
        }

        [Fact]
        public void OneToManyMapper_RemoveParent_ShouldThrowsException_WhenParentDoesNotExist()
        {
            var parentToRemoveThatDoesNotExist = 4;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10, 11 } },
                { 2, new HashSet<int> { 20, 21 } },
                { 3, new HashSet<int> { 30, 31 } }
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            Assert.Throws<ArgumentException>(() => oneToManyMapper.RemoveParent(parentToRemoveThatDoesNotExist));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData((_maxNumberAllowed + 1))]
        public void OneToManyMapper_RemoveParent_ShouldThrowsException_WhenNotValidEntry(int parent)
        {
            var oneToManyMapper = new OneToManyMapper();

            Assert.Throws<ArgumentException>(() => oneToManyMapper.RemoveParent(parent));
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData((_maxNumberAllowed + 1), 1)]
        [InlineData(1, (_maxNumberAllowed + 1))]
        public void OneToManyMapper_UpdateChild_ShouldThrowsException_WhenNotValidEntry(int oldChild, int newChild)
        {
            var oneToManyMapper = new OneToManyMapper();

            Assert.Throws<ArgumentException>(() => oneToManyMapper.UpdateChild(oldChild, newChild));
        }

        [Fact]
        public void OneToManyMapper_UpdateChild_ShouldUpdatesCorrectly_WhenOldChildExists()
        {
            var oldChild = 31;
            var newChild = 32;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10, 11 } },
                { 2, new HashSet<int> { 20, 21 } },
                { 3, new HashSet<int> { 30, oldChild } }
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            oneToManyMapper.UpdateChild(oldChild, newChild);

            Assert.Equal([30, newChild], initialSample[3]);
        }


        [Fact]
        public void OneToManyMapper_UpdateChild_ShouldDoesNothing_WhenOldChildDoestNotExist()
        {
            var oldChild = 33;
            var newChild = 34;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10, 11 } },
                { 2, new HashSet<int> { 20, 21 } },
                { 3, new HashSet<int> { 30 } }
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            oneToManyMapper.UpdateChild(oldChild, newChild);

            Assert.Equal([30], initialSample[3]);
        }


        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData((_maxNumberAllowed + 1), 1)]
        [InlineData(1, (_maxNumberAllowed + 1))]
        public void OneToManyMapper_UpdateParent_ShouldThrowsException_WhenNotValidEntry(int oldParent, int newParent)
        {
            var oneToManyMapper = new OneToManyMapper();

            Assert.Throws<ArgumentException>(() => oneToManyMapper.UpdateParent(oldParent, newParent));
        }

        [Fact]
        public void OneToManyMapper_UpdateChild_ShouldUpdatesCorrectly_WhenOldParentExists_ButNewParentDoesNot()
        {
            var oldParent = 3;
            var newParent = 4;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10, 11 } },
                { 2, new HashSet<int> { 20, 21 } },
                { oldParent, new HashSet<int> { 30, 31 } }
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            oneToManyMapper.UpdateParent(oldParent, newParent);

            Assert.Equal([30, 31], initialSample[newParent]);
        }

        [Fact]
        public void OneToManyMapper_UpdateChild_ShouldUpdatesCorrectly_WhenOldParentAndNewParentExist()
        {
            var oldParent = 3;
            var newParent = 4;

            var initialSample = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 10, 11 } },
                { 2, new HashSet<int> { 20, 21 } },
                { oldParent, new HashSet<int> { 30, 31 } },
                { newParent, new HashSet<int> { 40, 41 } }
            };

            var oneToManyMapper = new OneToManyMapper(initialSample);

            oneToManyMapper.UpdateParent(oldParent, newParent);

            Assert.Equal([30, 31, 40, 41], initialSample[newParent]);
            Assert.Equal([], initialSample[oldParent]);
        }
    }
}