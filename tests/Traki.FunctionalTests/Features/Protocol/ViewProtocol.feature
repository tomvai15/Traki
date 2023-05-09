Feature: View protocol

Scenario: Open protocol tab displays protocols
    Given I have logged in as project manager
    When I press on Projects tab
    Then projects should be displayed