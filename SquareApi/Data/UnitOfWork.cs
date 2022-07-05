﻿using SquareApi.Data.Contracts;
using SquareApi.Models;

namespace SquareApi.Data;

public class UnitOfWork : IUnitofWork, IDisposable
{
    private SquareApiDbContext _context;

    public IRepositoryBase<Point> Points => new PointRepository(_context);
    public IRepositoryBase<Square> Squares => new SquareRepository(_context);

    public UnitOfWork(SquareApiDbContext context) => _context = context;

    public void Dispose() => _context.Dispose();

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}