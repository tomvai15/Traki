
Feature: Update section

@ignore
Scenario: Update section with valid fields
    Given I have logged in as project manager
    And I have navigated to protocol templates page
    When I open edit section page
    And update question names
    Then template section should be updated


Scenario: Update section with invalid fields
    Given I have logged in as project manager
    And I have navigated to protocol templates page
    When I open edit section page
    And update question names with invalid characters
    Then I should not be allowed to update section