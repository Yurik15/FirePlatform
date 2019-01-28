using AutoMapper;
using FirePlatform.Models.Enums;
using FirePlatform.Models.Models;
using FirePlatform.Services;
using FirePlatform.WebApi.Model.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FirePlatform.WebApi.Controllers
{
    [ApiController]
    public class ItemsController : BaseController
    {
        public ItemsController
            (
                Service service,
                IMapper mapper
            )
            : base(service, mapper) { }

        [HttpGet("api/[controller]")]
        public VirtualFileResult GetVirtualFile()
        {
            var filepath = Path.Combine("~/Files", "test.xml");
            return File(filepath, "xml/application", "test.xml");
        }

        //TODO remove it after tests
        [HttpGet("api/[controller]/LoadTests")]
        [ProducesResponseType(200, Type = typeof(FormTreeResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        public OkObjectResult LoadTests(uint dataCount = 10)
        {
            var itemsGroups = GetTestsData((int)dataCount);

            return Ok(itemsGroups);
        }

        [HttpGet("api/[controller]/RecalculateTests")]
        [ProducesResponseType(200, Type = typeof(FormTreeResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        public OkObjectResult RecalculateTests(int id, string value)
        {
            var itemsToReturn = new List<Item>();

            var itemsGroups = GetTestsData();

            foreach (var itemGr in itemsGroups)
            {
                if (itemsToReturn.Count() > 0)
                    break;

                var editedElement = itemGr.Items.Where(x => x.Id == id).FirstOrDefault();
                var nextID = itemGr.Items.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;

                switch (editedElement.Title)
                {
                    case "CheckBox Add new object":
                        var newItem = new Item()
                        {
                            Id = nextID,
                            Value = 10,
                            Title = "Andrew years old",
                            InputType = ItemInputTypes.Number,
                            IsVisible = true,
                            ParentId = editedElement.ParentId
                        };
                        itemsToReturn.Add(newItem);
                        break;
                    case "CheckBox Remove new object":
                        var elToDelete = itemGr.Items.Where(x => x.Title == "To Delete").FirstOrDefault();
                        elToDelete.IsVisible = false;
                        itemsToReturn.Add(elToDelete);
                        break;
                    default:
                        try
                        {
                            var editedElementSum = itemGr.Items.Where(x => x.Title == "Summary").FirstOrDefault();
                            editedElement.Value = Int32.Parse(value);
                            var editedElement1 = itemGr.Items.Where(x => x.Title == "Number1").FirstOrDefault();
                            var editedElement2 = itemGr.Items.Where(x => x.Title == "Number2").FirstOrDefault();
                            editedElementSum.Value = (Int32.Parse(editedElement1.Value.ToString()) + Int32.Parse(editedElement2.Value.ToString())).ToString();
                            itemsToReturn.Add(editedElement);
                            itemsToReturn.Add(editedElementSum);
                            break;
                        }
                        catch (Exception ex)
                        { }
                        break;
                }
            }

            return Ok(itemsToReturn);
        }

        private List<ItemGroup> GetTestsData(int dataCount = 10)
        {
            var itemsGroups = new List<ItemGroup>();
            for (int i = 0; i < dataCount; i++)
            {
                var items = new List<Item>()
                        {
                            new Item()
                            {
                                Id = 1,
                                Value = 10,
                                Title = "Number",
                                InputType = ItemInputTypes.Number,
                                IsVisible = true
                            },
                             new Item()
                            {
                                Id = 2,
                                Value = false,
                                Title = "To Delete",
                                InputType = ItemInputTypes.Condition,
                                IsVisible = true
                            },
                            new Item()
                            {
                                Id = 3,
                                Value = "ALAMAKOTA",
                                Title = "Text",
                                InputType = ItemInputTypes.Text,
                                IsVisible = true
                            },
                            new Item()
                            {
                                Id = 4,
                                Value = false,
                                Title = "CheckBox Add new object",
                                InputType = ItemInputTypes.Condition,
                                IsVisible = true
                            },
                            new Item()
                            {
                                Id = 5,
                                Value = false,
                                Title = "CheckBox Remove new object",
                                InputType = ItemInputTypes.Condition,
                                IsVisible = true
                            },
                            new Item()
                            {
                                Id = 6,
                                Value = 123123,
                                Title = "Number1",
                                InputType = ItemInputTypes.Number,
                                IsVisible = true
                            },
                            new Item()
                            {
                                Id = 7,
                                Value = 10,
                                Title = "Number2",
                                InputType = ItemInputTypes.Number,
                                IsVisible = true
                            },
                                                        new Item()
                            {
                                Id = 8,
                                Value = 10,
                                Title = "Summary",
                                InputType = ItemInputTypes.Number,
                                IsVisible = true
                            },
                            new Item()
                            {
                                Id = 9,
                                Value = new string[]{"green", "blue", "red", "white"},
                                Title = "Dropdown control",
                                InputType = ItemInputTypes.DropDown,
                                IsVisible = true
                            },
                            new Item()
                            {
                                Id = 10,
                                Value = new string[]{"green", "blue", "red", "white"},
                                Title = "Multiple dropdown control",
                                InputType = ItemInputTypes.MultipleDropDown,
                                IsVisible = true
                            },
                            new Item()
                            {
                                Id = 11,
                                Value = new string[]{"green", "blue", "red", "white"},
                                Title = "Radio control",
                                InputType = ItemInputTypes.Radio,
                                IsVisible = true
                            },
                            new Item()
                            {
                                Id = 12,
                                Value = "ALAMAKOTA ALAMAKOTA ALAMAKOTA",
                                Title = "Tooltip",
                                InputType = ItemInputTypes.ToolTip,
                                IsVisible = true,
                                TooltipText = "ALAMAKOTA ALAMAKOTA ALAMAKOTA ALAMAKOTA ALAMAKOTA ALAMAKOTA" +
                                " ALAMAKOTA ALAMAKOTA ALAMAKOTA ALAMAKOTA ALAMAKOTA ALAMAKOTA"                                
                            }
                        };
                var itemGroup = new ItemGroup
                {
                    Title = "Pomieszczenie",
                    Items = items
                };
                itemsGroups.Add(itemGroup);
            }
            int counter = 0;
            int id = 10000;
            foreach (var itemGr in itemsGroups)
            {
                counter++;
                id++;
                itemGr.Id = id;
                itemGr.Title += " " + counter;

                foreach (var it in itemGr.Items)
                {
                    it.ParentId = itemGr.Id;
                    it.Id = it.Id * itemGr.Id + 1;
                }
            }

            return itemsGroups;
        }
    }
}
