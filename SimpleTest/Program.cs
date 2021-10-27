using JsonSerializable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTest {

	public interface IGroupedData : IJsonSerializable {

		public string Name { get; }

		public int Value { get; }

	}

	public class Category : IGroupedData {

		public List<IGroupedData> elements = new List<IGroupedData>();
		public string Name { get; set; }
		public int Value => elements.Sum(x => x.Value);

		public Category() {
		}

		public JsonData SaveToJson() {
			JsonObject group = new JsonObject();
			group["name"] = (JsonString)(Name ?? "null");

			JsonArray children = new JsonArray();
			foreach (IGroupedData element in elements) {
				children.Add(element.SaveToJson());
			}
			group["children"] = children;

			return group;
		}

		public void LoadFromJson(JsonData Data) {
			if(Data is JsonObject obj) {
				this.Name = (JsonString)obj["name"];

				JsonArray children = (JsonArray)obj["children"];
				foreach(JsonData child in children) {
					JsonObject childObj = (JsonObject)child;
					if(childObj["children"] != null) {
						Category cat = new Category();
						cat.LoadFromJson(childObj);
						this.elements.Add(cat);
					} else {
						GroupValue val = new GroupValue();
						val.LoadFromJson(childObj);
						this.elements.Add(val);
					}
				}
			}
		}
	}

	public class GroupValue : IGroupedData {
		public string Name { get; set; }
		public int Value { get; set; }
		public GroupValue() { }
		public JsonData SaveToJson() {
			JsonObject obj = new JsonObject();
			obj["name"] = (JsonString)(Name ?? "null");
			obj["value"] = (JsonInteger)Value;
			return obj;
		}

		public void LoadFromJson(JsonData Data) {
			if (Data is JsonObject obj) {
				this.Name = (JsonString)obj["name"];
				this.Value = (int)(JsonInteger)obj["value"];
			}
		}
	}

	class Program {
		static void Main(string[] args) {
			string text = "{" +
				"\"name\": \"flare\"," +
				"\"children\": [" +
				"	{" +
				"		\"name\": \"bob\"," +
				"		\"value\": 27" +
				"	}" +
				"]" +
			"}";

			Category cat = new Category(); 
			Json.FromString(text, cat);

			Console.WriteLine(Json.GetString(cat, true));
		}
	}
}
