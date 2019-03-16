using System;
using System.Collections.Generic;
using Amazon.S3;
using S3Batcher.Operations;

namespace S3Batcher.CommandLine
{
    static class Container
    {
        private static Dictionary<Type, Func<object>> _registrations = new Dictionary<Type, Func<object>>
        {
            { typeof(DeleteVersions), () => new DeleteVersions((AmazonS3Client)_registrations[typeof(AmazonS3Client)]())},
            { typeof(RestoreObjects), () => new RestoreObjects((AmazonS3Client)_registrations[typeof(AmazonS3Client)]())},
        };

        public static void Register(Type type, object obj)
        {
            _registrations.Add(type, () => obj);
        }

        public static object Resolve(Type type)
        {
            return _registrations[type]();
        }

        public static T Resolve<T>()
        {
            return (T)_registrations[typeof(T)]();
        }
    }
}