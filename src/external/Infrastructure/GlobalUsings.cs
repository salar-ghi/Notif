﻿global using Core;
global using AutoMapper;
global using Domain.Enums;
global using Domain.Entities;
global using FluentEmail.Core;
global using System.Text.Json;
global using Core.Abstractions;
global using System.Reflection;
global using Application.Models;
global using Domain.Entities.Base;
global using Application.Models.Requests;
global using Application.Models.Responses;
global using Microsoft.Extensions.Logging;
global using Microsoft.EntityFrameworkCore;
global using Application.Services.Abstractions;

global using Microsoft.Extensions.Caching.Memory;  // ??????????
global using Application.Services.Abstractions.HttpClients.ThirdParties;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Extensions.Caching.StackExchangeRedis;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Infrastructure.Persistence.Providers.EntityFramework.Context;
global using Infrastructure.Persistence.Providers.EntityFramework.Configurations;
global using Infrastructure.Persistence.Providers.EntityFramework;

global using Infrastructure.Services.ThirdParties;
global using Microsoft.Extensions.DependencyInjection;
global using Application.Services.Abstractions.ThirdParties;
global using Microsoft.Extensions.Configuration;