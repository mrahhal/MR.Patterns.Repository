# MR.Patterns.Repository

[![Build status](https://img.shields.io/appveyor/ci/mrahhal/mr-patterns-repository/master.svg)](https://ci.appveyor.com/project/mrahhal/mr-patterns-repository)
[![NuGet version](https://badge.fury.io/nu/MR.Patterns.Repository.svg)](https://www.nuget.org/packages/MR.Patterns.Repository)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)

Provides a base when implementing the repository pattern with EF6.

(Version 3.1 and above supports EF 6.4)

## Objectives

- Quickly implement the pattern with little to no plumbing or boilerplate on your end.
- Ease out unit testing by providing some kind of an in memory repository store.

To that end:

- Have a base repository class that handles most of the plumbing (like implementing Add, Remove, Update, SaveChanges, Transactions, ...).
- Have another base repository class that in memory implementations should inherit from. It should basically be an in memory store for entities that also provides things like ensuring (int, long) PKs get a proper incrementing value when entities are added.

## The basics

`IRepositoryCore`, `RepositoryCore`, and `InMemoryRepositoryCore` are provided for your (real) Repository classes to inherit from. They do most of the work.

## A simple example

```cs
// This is your service.
public interface IRepository : IRepositoryCore
{
    IQueryable<Blog> Blogs { get; }
}
```

```cs
// This is the real repository that stores entities in a database.
public class Repository : RepositoryCore<ApplicationDbContext>, IRepository
{
    public Repository(ApplicationDbContext context) : base(context) { }

    IQueryable<Blog> Blogs => Context.Blogs;

    // Add, Remove, Update, SaveChangesAsync are all implemented in RepositoryCore.
}
```

```cs
// This is the in memory repository that will depend on InMemoryRepositoryCore to store entities in memory.
public class InMemoryRepository : InMemoryRepositoryCore, IRepository
{
    // For() is a method on InMemoryRepositoryCore
    IQueryable<Blog> Blogs => For<Blog>();

    // Add, Remove, Update, SaveChangesAsync are all implemented in InMemoryRepositoryCore.
}
```

Note: The `InMemoryRepository` requires the PK property to be called "Id" for auto incrementing to work.

## Nice improvements to have

- Relationship fixups for the in memory store.
