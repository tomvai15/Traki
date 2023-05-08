Feature: Update project

Scenario: Create project with valid fields
    Given I have logged in as project manager
    And I have navigated to projects page
    When I open create project page
    And I add all project fields
    Then project is created