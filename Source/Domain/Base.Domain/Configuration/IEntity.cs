namespace Base.Domain.Configuration;
/// <summary>
/// سیستم IEntity  این امکان را فراهم میکند که به تمام مدل های سیستم یک دسترسی مشترک داشت و یک امکان را به همه مدل ها تزریق کرد 
/// </summary>
public interface IEntity
{
}

/// <summary>
/// سیستم IEntity  این امکان را فراهم میکند که به تمام مدل های سیستم یک دسترسی مشترک داشت و یک امکان را به همه مدل ها تزریق کرد 
/// </summary>
/// <typeparam name="TKey">با استفاده از این کلید میتوان نوع آیدی مدل را از بیرون به خود مدل تزریق  کرد</typeparam>
public interface IEntity<TKey> : IEntity
{
    TKey Id { get; set; }
}

public abstract class BaseEntity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; }
}

public abstract class BaseEntity : BaseEntity<int>
{ 
}