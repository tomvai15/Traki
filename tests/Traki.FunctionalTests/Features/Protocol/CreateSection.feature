@ignore
Feature: Create section

Scenario: Create section with all question types
    Given I have logged in as project manager
    And I have navigated to protocol templates page
    When I open create section page
    And add text input question
    And add multiple choice question
    And press section creation button
    Then section should be created