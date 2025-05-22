namespace DeepSpaceSaga.Tests.CommonTests.Extensions.Object;

public class ObjectExtensionsTests
{
    private class TestObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public NestedObject? Nested { get; set; }
    }

    private class NestedObject
    {
        public string Value { get; set; } = string.Empty;
    }

    [Fact]
    public void DeepClone_NullObject_ReturnsNull()
    {
        TestObject? obj = null;
        var clone = obj.DeepClone();
        Assert.Null(clone);
    }

    [Fact]
    public void DeepClone_SimpleObject_ReturnsEqualButNotSameReference()
    {
        var obj = new TestObject { Id = 1, Name = "Test" };
        var clone = obj.DeepClone();
        Assert.NotNull(clone);
        Assert.Equal(obj.Id, clone!.Id);
        Assert.Equal(obj.Name, clone.Name);
        Assert.NotSame(obj, clone);
    }

    [Fact]
    public void DeepClone_NestedObject_ClonesNestedProperties()
    {
        var obj = new TestObject
        {
            Id = 2,
            Name = "Parent",
            Nested = new NestedObject { Value = "Child" }
        };
        var clone = obj.DeepClone();
        Assert.NotNull(clone);
        Assert.NotNull(clone!.Nested);
        Assert.Equal(obj.Nested!.Value, clone.Nested!.Value);
        Assert.NotSame(obj.Nested, clone.Nested);
    }

    [Fact]
    public void DeepClone_CyclicReference_PreservesReference()
    {
        var a = new CyclicObject { Name = "A" };
        var b = new CyclicObject { Name = "B" };
        a.Reference = b;
        b.Reference = a;
        var clone = a.DeepClone();
        Assert.NotNull(clone);
        Assert.NotNull(clone!.Reference);
        Assert.Equal("A", clone.Name);
        Assert.Equal("B", clone.Reference!.Name);
        // Проверяем, что цикл сохранился
        Assert.Same(clone, clone.Reference.Reference);
    }

    private class CyclicObject
    {
        public string Name { get; set; } = string.Empty;
        public CyclicObject? Reference { get; set; }
    }
}