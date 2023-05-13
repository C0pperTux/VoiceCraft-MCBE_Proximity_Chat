﻿using Xamarin.Essentials;
using Newtonsoft.Json;
using System.IO;
using VoiceCraft_Android.Models;
using System;
using System.Linq;

namespace VoiceCraft_Android.Storage
{
    public static class Database
    {
        private const string DbFolder = "Databases";
        private const string ServersDb = "Servers.json";
        private const string SettingsDb = "Settings.json";

        private static string LocalPath;
        private static string ServersDbPath;
        private static string SettingsDbPath;

        static Database()
        {
            LocalPath = GetLocalFileDirectory();
            ServersDbPath = Path.Combine(LocalPath, ServersDb);
            SettingsDbPath = Path.Combine(LocalPath, SettingsDb);
        }

        public static ServerListModel GetServers()
        {
            ServerListModel servers = new ServerListModel();
            if (File.Exists(ServersDbPath))
            {
                var json = File.ReadAllText(ServersDbPath);
                servers = JsonConvert.DeserializeObject<ServerListModel>(json);
            }
            return servers;
        }

        public static void AddServer(ServerModel server)
        {
            var servers = GetServers();

            if(servers.Servers.Exists(x => x.Name == server.Name)) 
                throw new InvalidOperationException("Conflict detected! Multiple server objects cannot have the same name!");

            servers.Servers.Add(server);
            
            var serialized = JsonConvert.SerializeObject(servers);
            File.WriteAllText(ServersDbPath, serialized);
        }

        public static void DeleteServer(ServerModel server) 
        {
            var servers = GetServers();

            if (!servers.Servers.Exists(x => x.Name == server.Name))
                throw new InvalidOperationException("Cannot find server.");

            servers.Servers.RemoveAll(x => x.Name == server.Name);

            var serialized = JsonConvert.SerializeObject(servers);
            File.WriteAllText(ServersDbPath, serialized);
        }

        public static void EditServer(ServerModel server)
        {
            var servers = GetServers();

            var serverIndex = servers.Servers.FindIndex(x => x.Name == server.Name);
            if(serverIndex == -1)
                throw new InvalidOperationException("Cannot find server.");
            else
                servers.Servers[serverIndex] = server;

            var serialized = JsonConvert.SerializeObject(servers);
            File.WriteAllText(ServersDbPath, serialized);
        }

        public static ServerModel? GetServerByName(string name)
        {
            var servers = GetServers();

            return servers.Servers.FirstOrDefault(x => x.Name == name);
        }

        public static SettingsModel GetSettings()
        {
            var settings = new SettingsModel();
            if (File.Exists(SettingsDbPath))
            {
                var json = File.ReadAllText(SettingsDbPath);
                settings = JsonConvert.DeserializeObject<SettingsModel>(json);
            }
            return settings;
        }

        public static void SaveSettings(SettingsModel settings)
        {
            var serialized = JsonConvert.SerializeObject(settings);
            File.WriteAllText(SettingsDbPath, serialized);
        }


        private static string GetLocalFileDirectory()
        {
            var docFolder = FileSystem.AppDataDirectory;
            var libFolder = Path.Combine(docFolder, DbFolder);

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }
            return libFolder;
        }
    }
}
