using System;
using System.Collections.Generic;

namespace VoiceCraft.Server
{
    public class CommandHandler
    {
        private static readonly Dictionary<string, Action<string[]>> Commands = new();

        public static void RegisterCommand(string name, Action<string[]> action) => Commands[name.ToLower()] = action;

        public static void ParseCommand(string command)
        {
            string[] parts = command.Split(' ');
            string name = parts[0].ToLower();
            string[] args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();

            if (Commands.TryGetValue(name, out Action<string[]> cmd))
                cmd(args);
            else
                throw new Exception("Invalid command. Type 'help' for a list of available commands.");
        }
    }
}
