Feature: View defect

Scenario: Defects screen displays defects
    Given I have logged in as project manager
    When I press on Projects tab
    Then projects should be displayed