describe("Backend Test Spec", () => {
  const apiUrl = "https://localhost:44329";
  const unknownId = "10000000-0000-0000-0000-000000000000";
  const invalidId = "00000000-0000-0000-0000-000000000000";
  const invalidContentType = "text/plain";

  const headers = {
    "Content-Type": "application/json",
  };

  const HTTP_STATUS = {
    OK: 200,
    CREATED: 201,
    NO_CONTENT: 204,
    BAD_REQUEST: 400,
    NOT_FOUND: 404,
    CONFLICT: 409,
    UNSUPPORTED_MEDIA_TYPE: 415,
  };

  const testPolls = [
    {
      question: "Which of the countries would you like to visit the most?",
      options: [
        { key: "A", value: "Argentina" },
        { key: "B", value: "Austria" },
        { key: "C", value: "Australia" },
      ],
    },
    {
      question: "What's your favorite pet?",
      options: [
        { key: "A", value: "Dog" },
        { key: "B", value: "Cat" },
        { key: "C", value: "Crocodile" },
      ],
    },
  ];

  beforeEach(() => {
    cy.intercept({
      method: "GET",
      url: apiUrl,
    }).as("pingRequest");
  });

  const createPresentation = (polls) => {
    return cy.request({
      failOnStatusCode: false,
      method: "POST",
      url: `${apiUrl}/presentations`,
      headers,
      body: { polls },
    });
  };

  const getCurrentPoll = (presentationId) => {
    return cy.request({
      failOnStatusCode: false,
      method: "GET",
      url: `${apiUrl}/presentations/${presentationId}/polls/current`,
      headers,
    });
  };

  it("should call ping", () => {
    cy.request({
      method: "GET",
      url: `${apiUrl}/ping`,
    }).then((response) => {
      expect(response.status).to.eq(
        HTTP_STATUS.OK,
        `The application should respond with ${HTTP_STATUS.OK} on ping request. apiUrl: ${apiUrl}`
      );
    });
  });

  it(`creating a presentation with invalid data should result in ${HTTP_STATUS.BAD_REQUEST} status code`, () => {
    cy.request({
      failOnStatusCode: false,
      method: "POST",
      url: `${apiUrl}/presentations`,
      headers,
      body: {},
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.BAD_REQUEST,
        `Response status should be ${HTTP_STATUS.BAD_REQUEST} when creating a template with no polls`
      );
    });
  });

  it("should handle case where specified content type is not allowed", () => {
    cy.request({
      method: "POST",
      url: `${apiUrl}/presentations`,
      headers: {
        "Content-Type": invalidContentType,
      },
      failOnStatusCode: false,
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.UNSUPPORTED_MEDIA_TYPE,
        `Response status is ${HTTP_STATUS.UNSUPPORTED_MEDIA_TYPE} when invalid content type is used`
      );
    });
  });

  it("should create a new presentation", () => {
    createPresentation(testPolls).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.CREATED,
        `Response status is ${HTTP_STATUS.CREATED} when creating a presentation`
      );
      assert.isDefined(
        response.body.presentation_id,
        "Presentation Id is present when creating a presentation"
      );
    });
  });

  it(`reading a presentation using an unknown presentation id should result in ${HTTP_STATUS.NOT_FOUND} status code`, () => {
    cy.request({
      failOnStatusCode: false,
      method: "GET",
      url: `${apiUrl}/presentations/${unknownId}`,
      headers,
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.NOT_FOUND,
        `A ${HTTP_STATUS.NOT_FOUND} status code should be returned in case the presentation with the given id is not found`
      );
    });
  });

  it(`reading a presentation using a null presentation id should result in ${HTTP_STATUS.BAD_REQUEST} status code`, () => {
    cy.request({
      failOnStatusCode: false,
      method: "GET",
      url: `${apiUrl}/presentations/${invalidId}`,
      headers,
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.BAD_REQUEST,
        `A ${HTTP_STATUS.BAD_REQUEST} status code should be returned in case the presentation id is null`
      );
    });
  });

  it("should create a new presentation and return polls", () => {
    createPresentation(testPolls).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.CREATED,
        `Response status is ${HTTP_STATUS.CREATED} when creating a presentation`
      );
      assert.isDefined(
        response.body.presentation_id,
        "Presentation Id is present when creating a presentation"
      );

      const presentationId = response.body.presentation_id;

      cy.request({
        failOnStatusCode: false,
        method: "GET",
        url: `${apiUrl}/presentations/${presentationId}`,
        headers,
      }).then((response) => {
        assert.equal(
          response.status,
          HTTP_STATUS.OK,
          `Reading polls should return ${HTTP_STATUS.OK} status code`
        );
        expect(response.body.polls[0].question).to.deep.equal(
          "Which of the countries would you like to visit the most?"
        );
        expect(response.body.polls[1].question).to.deep.equal(
          "What's your favorite pet?"
        );
      });
    });
  });

  it(`reading a presentation's current poll using an unknown presentation id should result in ${HTTP_STATUS.CONFLICT} status code`, () => {
    cy.request({
      failOnStatusCode: false,
      method: "GET",
      url: `${apiUrl}/presentations/${unknownId}/polls/current`,
      headers,
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.CONFLICT,
        `A ${HTTP_STATUS.CONFLICT} status code should be returned in case the presentation with the given id is not found`
      );
    });
  });

  it(`reading a presentation using an null presentation id should result with ${HTTP_STATUS.BAD_REQUEST}  status code`, () => {
    cy.request({
      failOnStatusCode: false,
      method: "GET",
      url: `${apiUrl}/presentations/${invalidId}/polls/current`,
      headers,
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.BAD_REQUEST,
        `A ${HTTP_STATUS.BAD_REQUEST} status code should be returned in case of presentation of given id null`
      );
    });
  });

  it(`setting up current polls using an unknown presentation id should result in ${HTTP_STATUS.BAD_REQUEST} status code`, () => {
    cy.request({
      failOnStatusCode: false,
      method: "PUT",
      url: `${apiUrl}/presentations/${unknownId}/polls/current`,
      headers,
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.BAD_REQUEST,
        `A ${HTTP_STATUS.BAD_REQUEST} status code should be returned in case the presentation with the given id is not found`
      );
    });
  });

  it(`adding a vote using an unknown presentation id should result in ${HTTP_STATUS.NOT_FOUND} status code`, () => {
    cy.request({
      failOnStatusCode: false,
      method: "POST",
      url: `${apiUrl}/presentations/${unknownId}/polls/current/votes`,
      headers,
      body: {
        key: "A",
        client_id: "55555555-e89b-12d3-a456-426614174000",
        poll_id: invalidId,
      },
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.NOT_FOUND,
        `A ${HTTP_STATUS.NOT_FOUND} status code should be returned in case the presentation with the given id is not found`
      );
    });
  });

  it(`adding a vote using an empty body should result in ${HTTP_STATUS.BAD_REQUEST} status code`, () => {
    cy.request({
      failOnStatusCode: false,
      method: "POST",
      url: `${apiUrl}/presentations/${unknownId}/polls/current/votes`,
      headers,
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.BAD_REQUEST,
        `A ${HTTP_STATUS.BAD_REQUEST} status code should be returned in case the input body is null`
      );
    });
  });

  it(`getting votes using an unknown presentation id should result in ${HTTP_STATUS.NOT_FOUND} status code`, () => {
    cy.request({
      failOnStatusCode: false,
      method: "GET",
      url: `${apiUrl}/presentations/${unknownId}/polls/${unknownId}/votes`,
      headers,
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.NOT_FOUND,
        `A ${HTTP_STATUS.NOT_FOUND} status code should be returned in case the presentation with the given id is not found`
      );
    });
  });

  it(`getting votes using an invalid presentation id should result in ${HTTP_STATUS.BAD_REQUEST} status code`, () => {
    cy.request({
      failOnStatusCode: false,
      method: "GET",
      url: `${apiUrl}/presentations/${invalidId}/polls/${unknownId}/votes`,
      headers,
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.BAD_REQUEST,
        `A ${HTTP_STATUS.BAD_REQUEST} status code should be returned in case the presentation id is incorrect`
      );
    });
  });

  it(`getting votes using an invalid poll id should result in ${HTTP_STATUS.BAD_REQUEST} status code`, () => {
    cy.request({
      failOnStatusCode: false,
      method: "GET",
      url: `${apiUrl}/presentations/${unknownId}/polls/${invalidId}/votes`,
      headers,
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.BAD_REQUEST,
        `A ${HTTP_STATUS.BAD_REQUEST} status code should be returned in case the poll id is incorrect`
      );
    });
  });

  it(`reading a presentation using an invalid presentation id should result in ${HTTP_STATUS.BAD_REQUEST} status code`, () => {
    cy.request({
      failOnStatusCode: false,
      method: "GET",
      url: `${apiUrl}/presentations/${invalidId}/polls/current`,
      headers,
    }).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.BAD_REQUEST,
        `A ${HTTP_STATUS.BAD_REQUEST} status code should be returned in case the presentation id is invalid`
      );
    });
  });

  it("should create a presentation, switch to the current poll, vote, and read votes", () => {
    let presentationId, pollId;
    createPresentation(testPolls).then((response) => {
      assert.equal(
        response.status,
        HTTP_STATUS.CREATED,
        `Response status is ${HTTP_STATUS.CREATED} when creating a presentation`
      );
      assert.isDefined(
        response.body.presentation_id,
        "Presentation Id is present when creating a presentation"
      );

      presentationId = response.body.presentation_id;

      cy.request({
        failOnStatusCode: false,
        method: "PUT",
        url: `${apiUrl}/presentations/${presentationId}/polls/current`,
        headers,
      }).then((response) => {
        assert.equal(
          response.status,
          HTTP_STATUS.OK,
          `Switching to the next poll must return ${HTTP_STATUS.OK} HTTP code`
        );
        assert.isDefined(response.body.poll_id, "Poll id should be returned");
        assert.equal(
          response.body.question,
          "Which of the countries would you like to visit the most?"
        );
        assert.isUndefined(response.body.votes, "Votes should not be returned");

        pollId = response.body.poll_id;

        getCurrentPoll(presentationId).then((response) => {
          assert.equal(
            response.status,
            HTTP_STATUS.OK,
            `Getting current poll should return ${HTTP_STATUS.OK} status code`
          );
          assert.isDefined(response.body.poll_id, "Poll id should be returned");
          assert.equal(
            response.body.question,
            "Which of the countries would you like to visit the most?"
          );
          assert.isUndefined(
            response.body.votes,
            "Votes should not be returned"
          );
          assert.equal(pollId, response.body.poll_id);

          cy.request({
            failOnStatusCode: false,
            method: "POST",
            url: `${apiUrl}/presentations/${presentationId}/polls/current/votes`,
            headers,
            body: {
              key: "A",
              client_id: "55555555-e89b-12d3-a456-426614174000",
              poll_id: pollId,
            },
          }).then((response) => {
            assert.equal(
              response.status,
              HTTP_STATUS.NO_CONTENT,
              `Voting should return ${HTTP_STATUS.NO_CONTENT} status code`
            );

            cy.request({
              failOnStatusCode: false,
              method: "GET",
              url: `${apiUrl}/presentations/${presentationId}/polls/${pollId}/votes`,
              headers,
            }).then((response) => {
              assert.equal(
                response.status,
                HTTP_STATUS.OK,
                `Reading votes should return ${HTTP_STATUS.OK} status code`
              );
              expect(response.body).to.deep.equal([
                { client_id: "55555555-e89b-12d3-a456-426614174000", key: "A" },
              ]);
            });
          });
        });
      });
    });
  });
});
