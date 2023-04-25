import { ThemeOptions, createTheme } from '@mui/material/styles';

const colors = {
  // paper & background
  paper: '#ffffff',

  // primary
  primaryLight: "'#eef2f6",
  primaryMain: '#e2af3f',
  primaryDark: '#A66F15',
  primary200: '#E6AB4D',
  primary800: '#AB761F',

  // secondary
  secondaryLight: '#F6EEE7',
  secondaryMain: '#BDBAB4',
  secondaryDark: '#7B756A',
  secondary200: '#BDBAB4',
  secondary800: '#524B3F',

  // success Colors
  successLight: '#b9f6ca',
  success200: '#69f0ae',
  successMain: '#00e676',
  successDark: '#00c853',

  // error
  errorLight: '#ef9a9a',
  errorMain: '#f44336',
  errorDark: '#c62828',

  // orange
  orangeLight: '#fbe9e7',
  orangeMain: '#ffab91',
  orangeDark: '#d84315',

  // warning
  warningLight: '#fff8e1',
  warningMain: '#ffe57f',
  warningDark: '#ffc107',

  // grey
  grey50: '#F8FAFC',
  grey100: '#EEF2F6',
  grey200: '#E3E8EF',
  grey300: '#CDD5DF',
  grey500: '#697586',
  grey600: '#4B5565',
  grey700: '#364152',
  grey900: '#121926',

  // ==============================|| DARK THEME VARIANTS ||============================== //

  // paper & background
  darkBackground: '#1a223f', // level 3
  darkPaper: '#111936', // level 4

  // dark 800 & 900
  darkLevel1: '#29314f', // level 1
  darkLevel2: '#212946', // level 2

  // primary dark
  darkPrimaryLight: '#eef2f6',
  darkPrimaryMain: '#F3A321',
  darkPrimaryDark: '#E5881E',
  darkPrimary200: '#F9D290',
  darkPrimary800: '#C08115',

  // secondary dark
  darkSecondaryLight: '#E9D6C4',
  darkSecondaryMain: '#FFAF4D',
  darkSecondaryDark: '#FFA91F',
  darkSecondary200: '#DBC49D',
  darkSecondary800: '#EAA400',

  // text variants
  darkTextTitle: '#d7dcec',
  darkTextPrimary: '#F0DCBD',
  darkTextSecondary: '#C4AE84',
};

const colorScheme = {
  heading: colors.grey900,
  paper: colors.paper,
  backgroundDefault: colors.paper,
  background: colors.primaryLight,
  darkTextPrimary: colors.grey700,
  darkTextSecondary: colors.grey500,
  textDark: colors.grey900,
  menuSelected: colors.secondaryDark,
  menuSelectedBack: colors.secondaryLight,
  divider: colors.grey200,
};

export const theme = () => {
  const borderRadius = 10;
  const themeOptions: ThemeOptions = {
    direction: 'ltr',
    palette: themePalette(),
    mixins: {
      toolbar: {
        minHeight: '48px',
        padding: '16px',
        '@media (min-width: 600px)': {
          minHeight: '48px'
        }
      }
    },
  };

  const themes = createTheme(themeOptions);
  themes.components = componentStyleOverrides(borderRadius);

  return themes;
};

function themePalette() {
  return {
    common: {
      black: colors.darkPaper
    },
    primary: {
      light: colors.primaryLight,
      main: colors.primaryMain,
      dark: colors.primaryDark,
      200: colors.primary200,
      800: colors.primary800
    },
    secondary: {
      light: colors.secondaryLight,
      main: colors.secondaryMain,
      dark: colors.secondaryDark,
      200: colors.secondary200,
      800: colors.secondary800
    },
    error: {
      light: colors.errorLight,
      main: colors.errorMain,
      dark: colors.errorDark
    },
    orange: {
      light: colors.orangeLight,
      main: colors.orangeMain,
      dark: colors.orangeDark
    },
    warning: {
      light: colors.warningLight,
      main: colors.warningMain,
      dark: colors.warningDark
    },
    success: {
      light: colors.successLight,
      200: colors.success200,
      main: colors.successMain,
      dark: colors.successDark
    },
    grey: {
      50: colors.grey50,
      100: colors.grey100,
      500: colors.darkTextSecondary,
      600: colorScheme.heading,
      700: colors.darkTextPrimary,
      900: colorScheme.textDark
    },
    dark: {
      light: colors.darkTextPrimary,
      main: colors.darkLevel1,
      dark: colors.darkLevel2,
      800: colors.darkBackground,
      900: colors.darkPaper
    },
    text: {
      primary: colorScheme.darkTextPrimary,
      secondary: colorScheme.darkTextSecondary,
      dark: colorScheme.textDark,
      hint: colors.grey100
    },
    background: {
      paper: colorScheme.paper,
      default: colorScheme.backgroundDefault
    }
  };
}

function componentStyleOverrides(borderRadius: number) {
  const bgColor = colors.grey50;
  return {
    MuiButton: {
      styleOverrides: {
        root: {
          fontWeight: 500,
          borderRadius: '4px'
        }
      }
    },
    MuiPaper: {
      defaultProps: {
        elevation: 0
      },
      styleOverrides: {
        root: {
          backgroundImage: 'none'
        },
        rounded: {
          borderRadius: `${borderRadius}px`
        }
      }
    },
    MuiCardHeader: {
      styleOverrides: {
        root: {
          color: colorScheme.textDark,
          padding: '24px'
        },
        title: {
          fontSize: '1.125rem'
        }
      }
    },
    MuiCardContent: {
      styleOverrides: {
        root: {
          padding: '24px'
        }
      }
    },
    MuiCardActions: {
      styleOverrides: {
        root: {
          padding: '24px'
        }
      }
    },
    MuiListItemButton: {
      styleOverrides: {
        root: {
          color: colorScheme.darkTextPrimary,
          paddingTop: '10px',
          paddingBottom: '10px',
          '&.Mui-selected': {
            color: colorScheme.menuSelected,
            backgroundColor: colorScheme.menuSelectedBack,
            '&:hover': {
              backgroundColor: colorScheme.menuSelectedBack
            },
            '& .MuiListItemIcon-root': {
              color: colorScheme.menuSelected
            }
          },
          '&:hover': {
            backgroundColor: colorScheme.menuSelectedBack,
            color: colorScheme.menuSelected,
            '& .MuiListItemIcon-root': {
              color: colorScheme.menuSelected
            }
          }
        }
      }
    },
    MuiListItemIcon: {
      styleOverrides: {
        root: {
          color: colorScheme.darkTextPrimary,
          minWidth: '36px'
        }
      }
    },
    MuiListItemText: {
      styleOverrides: {
        primary: {
          color: colorScheme.textDark
        }
      }
    },
    MuiInputBase: {
      styleOverrides: {
        input: {
          color: colorScheme.textDark,
          '&::placeholder': {
            colorScheme: colorScheme.darkTextSecondary,
            fontSize: '0.875rem'
          }
        }
      }
    },
    MuiOutlinedInput: {
      styleOverrides: {
        root: {
          background: colors.grey50,
          borderRadius: `${borderRadius}px`,
          '& .MuiOutlinedInput-notchedOutline': {
            borderColor: colors.grey300
          },
          '&:hover $notchedOutline': {
            borderColor: colors.primaryLight
          },
          '&.MuiInputBase-multiline': {
            padding: 1
          }
        },
        input: {
          fontWeight: 500,
          background: bgColor,
          padding: '15.5px 14px',
          borderRadius: `${borderRadius}px`,
          '&.MuiInputBase-inputSizeSmall': {
            padding: '10px 14px',
            '&.MuiInputBase-inputAdornedStart': {
              paddingLeft: 0
            }
          }
        },
        inputAdornedStart: {
          paddingLeft: 4
        },
        notchedOutline: {
          borderRadius: `${borderRadius}px`
        }
      }
    },
    MuiSlider: {
      styleOverrides: {
        root: {
          '&.Mui-disabled': {
            color: colors.grey300
          }
        },
        mark: {
          backgroundColor: colors.paper,
          width: '4px'
        },
        valueLabel: {
          color: colors.primaryLight
        }
      }
    },
    MuiDivider: {
      styleOverrides: {
        root: {
          borderColor: colorScheme.divider,
          opacity: 1
        }
      }
    },
    MuiAvatar: {
      styleOverrides: {
        root: {
          color: colors.primaryDark,
          background: colors.primary200
        }
      }
    },
    MuiChip: {
      styleOverrides: {
        root: {
          '&.MuiChip-deletable .MuiChip-deleteIcon': {
            color: 'inherit'
          },
          '& .MuiChip-label': {
            overflow: 'visible !important',
          },
        },
      }
    },
    MuiTooltip: {
      styleOverrides: {
        tooltip: {
          color: colorScheme.paper,
          background: colors.grey700
        }
      }
    }
  };
}

export default theme;
