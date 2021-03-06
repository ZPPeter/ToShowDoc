﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ToShowDoc.Core.ShowDoc;

namespace ToShowDoc.Commands
{
    [Command(Name = "del", Description = "delete project")]
    [HelpOption("-h|--help")]
    public class DeleteCommand
    {
        [Required]
        [Option("-n|--name", CommandOptionType.SingleValue, Description = "Project Name")]
        public string Name { get; set; }

        private readonly IShowDocStore _showDocStore;

        public DeleteCommand(IShowDocStore showDocStore)
        {
            _showDocStore = showDocStore;
        }

        public async Task OnExecute(CommandLineApplication app)
        {
            await _showDocStore.DeleteShowDoc(x => x.Name == Name);
            Console.WriteLine("Delete Success");
        }
    }
}
