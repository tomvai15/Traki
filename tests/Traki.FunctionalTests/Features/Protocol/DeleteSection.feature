Feature: Delete section

Scenario: Delete section
    Given I have logged in as project manager
    And I have navigated to protocol templates page
    When I open edit section page
    And press delete button
    Then section should be deleted
