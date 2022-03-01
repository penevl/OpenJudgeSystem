import * as React from 'react';
import { useState } from 'react';
import { Box, createTheme, Tab } from '@material-ui/core';
import { TabContext, TabList, TabPanel } from '@material-ui/lab';
import { ThemeProvider } from '@mui/styles';
import { ThemeOptions } from '@material-ui/core/styles/createTheme';

interface ITabPanelProps {
    tabChildren: React.ReactNode[]
    tabLabels: string[]
    themeOverride?: ThemeOptions
}

const defaultState = { initialValue: '0' };

const Tabs = ({ tabChildren, tabLabels, themeOverride }: ITabPanelProps) => {
    const [ value, setValue ] = useState(defaultState.initialValue);

    const theme =
        themeOverride == null
            ? createTheme({ overrides: { MuiTabs: { indicator: { backgroundColor: '#42abf8', height: 3 } } } })
            : themeOverride;

    const handleChange = (event: React.ChangeEvent<{}>, newValue: string) => {
        setValue(newValue);
    };

    const renderTabs = () => tabLabels.map((tl: string, index: number) => (
        <Tab label={tl} value={index.toString()} />
    ));

    const renderTabChildren = () => tabChildren?.map((tc: React.ReactNode, index: number) => (
        <TabPanel value={index.toString()}>{tc}</TabPanel>
    ));

    return (
        <ThemeProvider theme={theme}>
            <Box sx={{ width: '100%' }}>
                <TabContext value={value}>
                    <Box>
                        <TabList
                          onChange={handleChange}
                        >
                            {renderTabs()}
                        </TabList>
                    </Box>
                    {renderTabChildren()}
                </TabContext>
            </Box>
        </ThemeProvider>
    );
};

export default Tabs;
