﻿using System;
using System.Collections.Generic;
using System.Linq;
using SimpleIdentityServer.Core.Models;
using SimpleIdentityServer.Core.Repositories;

using SimpleIdentityServer.DataAccess.Fake.Extensions;

namespace SimpleIdentityServer.DataAccess.Fake.Repositories
{
    public class FakeScopeRepository : IScopeRepository
    {
        private readonly FakeDataSource _fakeDataSource;
        
        public FakeScopeRepository(FakeDataSource fakeDataSource) 
        {
            _fakeDataSource = fakeDataSource;
        }
        
        public bool InsertScope(Core.Models.Scope scope)
        {
            _fakeDataSource.Scopes.Add(scope.ToFake());
            return true;
        }


        public Core.Models.Scope GetScopeByName(string name)
        {
            var scope = _fakeDataSource.Scopes.SingleOrDefault(s => s.Name == name);
            return scope.ToBusiness();
        }
        
        public IList<Core.Models.Scope> GetAllScopes()
        {
            return _fakeDataSource.Scopes.Select(s => s.ToBusiness()).ToList();
        }

        public bool DeleteScope(Scope scope)
        {
            throw new NotImplementedException();
        }

        public bool UpdateScope(Scope scope)
        {
            throw new NotImplementedException();
        }

        public bool AddScope(Scope scope)
        {
            throw new NotImplementedException();
        }
    }
}
