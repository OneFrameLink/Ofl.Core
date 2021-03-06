﻿using System.Threading.Tasks;

namespace Ofl.Core.Threading.Tasks
{
    public static class ValueTaskExtensions
    {
        public static ValueTask<T> FromResult<T>(T result) => new ValueTask<T>(result);
    }
}
