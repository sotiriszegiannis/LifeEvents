﻿using Domain;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    public class TagsRepository : BaseRepository<Tag>
    {
        public TagsRepository(IDbContextFactory<AppDbContext> dbContextFactory,ITenantResolver tenantResolver) : base(dbContextFactory, tenantResolver) { }
    }
}
