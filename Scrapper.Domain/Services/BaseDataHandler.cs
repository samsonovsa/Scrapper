﻿using Scrapper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scrapper.Domain.Services
{
    public abstract class BaseDataHandler<T> : IDataHandler<T>
    {
        public IDbContext DbContext { get; }

        public BaseDataHandler(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task HandleEntitiesAsync(List<T> entities)
        {
            entities = PreprocessingEntities(entities);

            foreach (var entity in entities)
            {
               await HandleEntity(entity);
            }

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
            }
        }

        public abstract List<T> PreprocessingEntities(List<T> entities);

        public abstract Task HandleEntity(T entity);
    }
}
