# JsonTranslator

Simple tool to translate the value of a JSON to a different language using Azure translator.

In this case, I'm going to translate a JSON with the next format from Azure Conversational Language Understanding projects.

```json
[
    {
        "text": "Text to translate",
        "language": "en-us",
        "intent": "Intent",
        "entities": [],
        "dataset": ""
    },
    {
        "text": "Text to translate",
        "language": "en-us",
        "intent": "Intent",
        "entities": [],
        "dataset": ""
    }
]
```
The translator API will give you the response in this format, and only takes the value of the text variable.

```json
[
	{
		"translations": [
			{
				"text": "Texto a traducir",
				"to": "es"
			}
		]
	},
	{
		"translations": [
			{
				"text": "Texto a traducir",
				"to": "es"
			}
		]
	}
]
```
To get back to the same format as the input, the result for the translator is in the same order as the input so for each value in the input change the old text for the translated one and in this case also change the language.