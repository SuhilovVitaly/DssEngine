﻿namespace DeepSpaceSaga.Common.Abstractions.Mappers.CelestialObjects
{
    public static class AsteroidMapper
    {
        public static ICelestialObject ToGameObject(CelestialObjectDto celestialObjectDto)
        {
            ICelestialObject asteroid = new BaseAsteroid(1)
            {
                Id = celestialObjectDto.Id,
                OwnerId = 0,
                Name = celestialObjectDto.Name,
                Direction = celestialObjectDto.Direction,
                X = celestialObjectDto.X,
                Y = celestialObjectDto.Y,
                Speed = celestialObjectDto.Speed,
                Type = CelestialObjectType.Asteroid,
                IsPreScanned = celestialObjectDto.IsPreScanned,
                Size = celestialObjectDto.Size
            };

            return asteroid;
        }
    }
}
