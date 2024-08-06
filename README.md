Implemented API endpoints:

1. `PUT api/presentations/{presentation_id}/polls/current`
2. `GET api/presentations/{presentation_id}/polls/current`
3. `POST api/presentations/{presentation_id}/polls/current/votes`
4. `GET api/presentations/{presentation_id}/polls/{poll_id}/votes`
5. `GET api/ping`

Used API `https://infra.devskills.app/api/interactive-presentation/v4`:

1. `POST /presentations`
2. `GET /presentations/{presentation_id}`

- [x] .Net8 APIs
- [x] [E2E tests](https://github.com/ffc1e12/interactive-presentation-polls-and-votes-backend-level-2_102e118-2e57g0/blob/1b121510b674f6b88313864eda80d68ec9d1f973/cypress/cypress/e2e/spec.cy.js)
- [x] Server data in a [SQLite]
- [x] xUnit test

![image](https://github.com/ffc1e12/interactive-presentation-polls-and-votes-backend-level-2_102e118-2e57g0/assets/170366343/330c227e-3edc-45fa-a8d9-4ceab34dbba8)

![image](https://github.com/ffc1e12/interactive-presentation-polls-and-votes-backend-level-2_102e118-2e57g0/assets/170366343/502922bd-de7a-48d7-a9ab-1147bba359e4)


# TASK
# Interactive Presentation Polls & Votes Backend - Level 2

There's a presenter on the stage. They ask the audience a series of questions. One poll per each slide. The people in the audience can vote as long as the current question is displayed.

<img width="550" src="https://user-images.githubusercontent.com/1162212/139849812-de799423-efc1-42f4-8298-e779c3aa17d7.png" />

## Time estimate

Between 2 and 3 hours, plus the time to set up the codebase.

## Mandatory steps before you get started

1. Set up your codebase according to [either of the outlined options](https://help.alvalabs.io/en/articles/9028914-how-to-set-up-the-codebase-for-your-coding-test) for your coding test.
2. Learn [how to get help](https://help.alvalabs.io/en/articles/9028899-how-to-ask-for-help-with-coding-tests) if you run into an issue with your coding test.

## The Task

<!--TASK_INSTRUCTIONS_START-->

The full system flows are visualised by the following [sequence diagram](https://swimlanes.io/u/mmSDwCQdM).

Your task is to **build a backend app** that **fulfills the Interactive Presentation API**.

<details>
<summary>Interactive Presentation API Specification</summary>

```json
{
  "openapi": "3.0.0",
  "info": {
    "title": "Interactive Presentations API",
    "version": "4.0.0"
  },
  "tags": [
    {
      "name": "Presenter",
      "description": "Operations used by the presenting webapp"
    },
    {
      "name": "Audience",
      "description": "Endpoints used by audience mobile app"
    },
    {
      "name": "Common",
      "description": "Reading current poll served to both presenter and the audience"
    },
    {
      "name": "Misc",
      "description": "Miscellaneous"
    }
  ],
  "servers": [
    {
      "url": "https://infra.devskills.app/api/interactive-presentation/v4"
    }
  ],
  "paths": {
    "/ping": {
      "get": {
        "summary": "Healhcheck to make sure the service is up",
        "responses": {
          "200": {
            "description": "The service is up and running"
          }
        },
        "tags": [
          "Misc"
        ]
      }
    },
    "/presentations": {
      "post": {
        "summary": "Creates a new presentation",
        "requestBody": {
          "required": true,
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/Presentation"
              }
            }
          }
        },
        "responses": {
          "201": {
            "description": "Presentation created.",
            "content": {
              "application/json": {
                "schema": {
                  "type": "object",
                  "properties": {
                    "presentation_id": {
                      "type": "string",
                      "format": "uuid",
                      "example": "123e4567-e89b-12d3-a456-426614174000"
                    }
                  }
                }
              }
            }
          },
          "400": {
            "description": "Mandatory body parameters missing or have incorrect type."
          },
          "405": {
            "description": "Specified HTTP method not allowed."
          },
          "415": {
            "description": "Specified content type not allowed."
          }
        },
        "tags": [
          "Presenter"
        ]
      }
    },
    "/presentations/{presentation_id}": {
      "parameters": [
        {
          "in": "path",
          "name": "presentation_id",
          "schema": {
            "type": "string",
            "format": "uuid",
            "example": "123e4567-e89b-12d3-a456-426614174000"
          },
          "required": true
        }
      ],
      "get": {
        "summary": "Reading the specified presentation",
        "responses": {
          "200": {
            "description": "Returning a Presentation object",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Presentation"
                }
              }
            }
          },
          "404": {
            "description": "There is no presentation with the provided `presentation_id`"
          },
          "409": {
            "description": "There are no polls currently displayed"
          }
        },
        "tags": [
          "Presenter"
        ]
      }
    },
    "/presentations/{presentation_id}/polls/current": {
      "parameters": [
        {
          "in": "path",
          "name": "presentation_id",
          "schema": {
            "type": "string",
            "format": "uuid",
            "example": "123e4567-e89b-12d3-a456-426614174000"
          },
          "required": true
        }
      ],
      "get": {
        "summary": "Reading currently presented poll for a given presentation",
        "responses": {
          "200": {
            "description": "Returning `description`, `poll_id` and `options`, mainly for the voter to vote",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Poll"
                }
              }
            }
          },
          "409": {
            "description": "There are no polls currently displayed"
          }
        },
        "tags": [
          "Common"
        ]
      },
      "put": {
        "summary": "Presenting the next poll",
        "responses": {
          "200": {
            "description": "The presentation successfully switched to the next slide. Responding with the poll content.",
            "content": {
              "application/json": {
                "schema": {
                  "allOf": [
                    {
                      "$ref": "#/components/schemas/Poll"
                    }
                  ]
                }
              }
            }
          },
          "404": {
            "description": "No presentation found."
          },
          "409": {
            "description": "The presentation ran out of polls."
          }
        },
        "tags": [
          "Presenter"
        ]
      }
    },
    "/presentations/{presentation_id}/polls/current/votes": {
      "parameters": [
        {
          "in": "path",
          "name": "presentation_id",
          "schema": {
            "type": "string",
            "format": "uuid",
            "example": "123e4567-e89b-12d3-a456-426614174000"
          },
          "required": true
        }
      ],
      "post": {
        "summary": "Sending votes",
        "requestBody": {
          "required": true,
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/Vote"
              }
            }
          }
        },
        "responses": {
          "204": {
            "description": "Vote successfully recorded"
          },
          "400": {
            "description": "The `poll_id` in the request body doesn't match the current poll."
          },
          "404": {
            "description": "Either `presentation_id` or `poll_id` not found"
          },
          "409": {
            "description": "In case of `poll_id` not matching currently displayed poll"
          }
        },
        "tags": [
          "Audience"
        ]
      }
    },
    "/presentations/{presentation_id}/polls/{poll_id}/votes": {
      "parameters": [
        {
          "in": "path",
          "name": "presentation_id",
          "schema": {
            "type": "string"
          },
          "required": true
        },
        {
          "in": "path",
          "name": "poll_id",
          "schema": {
            "type": "string"
          },
          "required": true
        }
      ],
      "get": {
        "summary": "Reading poll's voting results",
        "responses": {
          "200": {
            "description": "Vote successfully retrieved",
            "content": {
              "application/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/Vote"
                  }
                }
              }
            }
          },
          "404": {
            "description": "Either `presentation_id` or `poll_id` not found"
          },
          "409": {
            "description": "In case of `poll_id` not matching currently displayed poll"
          }
        },
        "tags": [
          "Presenter"
        ]
      }
    }
  },
  "components": {
    "schemas": {
      "Presentation": {
        "type": "object",
        "properties": {
          "current_poll_index": {
            "type": "integer",
            "readOnly": true
          },
          "polls": {
            "type": "array",
            "items": {
              "$ref": "#/components/schemas/Poll"
            }
          }
        },
        "required": [
          "current_poll_index",
          "polls"
        ]
      },
      "Option": {
        "type": "object",
        "properties": {
          "key": {
            "type": "string",
            "example": "A"
          },
          "value": {
            "type": "string",
            "example": "Argentina"
          }
        },
        "required": [
          "key",
          "value"
        ]
      },
      "Vote": {
        "type": "object",
        "properties": {
          "key": {
            "type": "string",
            "example": "A"
          },
          "client_id": {
            "type": "string",
            "format": "uuid",
            "example": "55555555-e89b-12d3-a456-426614174000"
          },
          "poll_id": {
            "type": "string",
            "format": "uuid",
            "example": "12225555-e89b-12d3-a456-426614174000",
            "writeOnly": true
          }
        },
        "required": [
          "key",
          "client_id",
          "poll_id"
        ]
      },
      "Poll": {
        "properties": {
          "poll_id": {
            "type": "string",
            "format": "uuid",
            "example": "123e4567-e89b-12d3-a456-426614174000",
            "readOnly": true
          },
          "question": {
            "type": "string",
            "example": "Which of the countries would you like to visit the most?"
          },
          "options": {
            "type": "array",
            "items": {
              "$ref": "#/components/schemas/Option"
            }
          }
        },
        "required": [
          "poll_id",
          "options",
          "question"
        ]
      }
    }
  }
}
```
</details>

However, you'll **only need to implement the following endpoints from scratch**:

1. `PUT /presentations/{presentation_id}/polls/current`
2. `GET /presentations/{presentation_id}/polls/current`
3. `POST /presentations/{presentation_id}/polls/current/votes`
4. `GET /presentations/{presentation_id}/polls/{poll_id}/votes`
5. `GET /ping`

For the endpoints listed below, please proxy the responses from `https://infra.devskills.app/api/interactive-presentation/v4`:

1. `POST /presentations`
2. `GET /presentations/{presentation_id}`

This means that you'll only need to store polls and votes in your database. And presentations will be fetched from `https://infra.devskills.app/api/interactive-presentation/v4` every time you need them.

### Tech Stack

Please agree with your hiring team regarding the tech stack choice.

### Solution expectations

- Do your best to make the [provided E2E tests](cypress/e2e/test.cy.js) pass. Check out [this tutorial](https://help.alvalabs.io/en/articles/9028831-how-to-work-with-cypress) to learn how to execute these tests and analyze the results.
- Keep server data in a [SQLite](https://www.sqlite.org/index.html) database. We want to see how you design the database schema and SQL queries.
- Avoid duplication and extract re-usable modules where it makes sense. We want to see your approach to creating a codebase that is easy to maintain.
- Unit test one module of choice. There is no need to test the whole app, as we only want to understand what you take into consideration when writing unit tests.

<!--TASK_INSTRUCTIONS_END-->

## When you are done

1. [Create a new Pull Request](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/creating-a-pull-request) from the branch where you've committed your solution to the default branch of this repository. **Please do not merge the created Pull Request**.
2. Go to your application in [Alva Labs](https://app.alvalabs.io) and submit your test.

---

Authored by [Alva Labs](https://www.alvalabs.io/).

Run E2E tests: `npx cypress open`