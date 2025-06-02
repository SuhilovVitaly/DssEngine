// Global using directives
global using System;
global using System.Collections.Concurrent;
global using System.Threading;
global using System.Threading.Tasks;
global using System.IO;
global using System.Drawing;
global using System.Linq;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Diagnostics.CodeAnalysis;

global using Microsoft.Extensions.DependencyInjection;

global using Xunit;
global using Moq;
global using FluentAssertions;

global using log4net;
global using log4net.Config;

global using DeepSpaceSaga.Common;
global using DeepSpaceSaga.Common.Abstractions.Entities;
global using DeepSpaceSaga.Common.Abstractions.Entities.Items;
global using DeepSpaceSaga.Common.Abstractions.Entities.Equipment;
global using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
global using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Asteroids;
global using DeepSpaceSaga.Common.Abstractions.Entities.Dialogs;
global using DeepSpaceSaga.Common.Abstractions.Entities.Characters;
global using DeepSpaceSaga.Common.Abstractions.Services;
global using DeepSpaceSaga.Common.Abstractions.Dto;
global using DeepSpaceSaga.Common.Tools;
global using DeepSpaceSaga.Common.Implementation.GameLoop;
global using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;
global using DeepSpaceSaga.Common.Abstractions.Session.Entities;
global using DeepSpaceSaga.Common.Geometry;
global using DeepSpaceSaga.Common.Extensions;
global using DeepSpaceSaga.Common.Extensions.Object;
global using DeepSpaceSaga.Common.Abstractions.Mappers;
global using DeepSpaceSaga.Common.Abstractions.Entities.Commands;
global using DeepSpaceSaga.Common.Abstractions.Events;

global using DeepSpaceSaga.Server;
global using DeepSpaceSaga.Server.Services;
global using DeepSpaceSaga.Server.Services.Metrics;
global using DeepSpaceSaga.Server.Services.SessionInfo;
global using DeepSpaceSaga.Server.Services.SessionContext;
global using DeepSpaceSaga.Server.Services.Scheduler;
global using DeepSpaceSaga.Server.Processing;