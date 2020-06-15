import React from 'react';
import { ResponsiveLine } from '@nivo/line'

var months = [ "ЯНВ", "ФЕВ", "МАР", "АПР", "МАЙ", "ИЮН", 
           "ИЮЛ", "АВГ", "СЕН", "ОКТ", "НОЯ", "ДЕК" ];

export default ({ data /* see data tab */ }) => (
    <ResponsiveLine
        data={data}
        margin={{ top: 5, right: 150, bottom: 50, left: 60 }}
        xScale={{ type: 'linear', min: 1, stacked: false}}
        yScale={{ type: 'linear', min: 'auto', max: 'auto', stacked: true, reverse: false }}
        curve="linear"
        axisTop={null}
        axisRight={null}
        axisBottom={{
            orient: 'bottom',
            tickSize: 5,
            tickPadding: 5,
            tickRotation: 0,
            legend: 'Месяц',
            legendOffset: 36,
            legendPosition: 'middle',
            format: value => months[value-1],
        }}
        axisLeft={{
            orient: 'left',
            tickSize: 5,
            tickPadding: 5,
            tickRotation: 0,
            legend: 'Аренды',
            legendOffset: -40,
            legendPosition: 'middle'
        }}
        colors={{ scheme: 'nivo' }}
        pointSize={10}
        pointColor={{ theme: 'background' }}
        pointBorderWidth={2}
        pointBorderColor={{ from: 'serieColor' }}
        pointLabel="y"
        pointLabelYOffset={-12}
        useMesh={true}
        legends={[
            {
                anchor: 'bottom-right',
                direction: 'column',
                justify: false,
                translateX: 100,
                translateY: 0,
                itemsSpacing: 0,
                itemDirection: 'left-to-right',
                itemWidth: 80,
                itemHeight: 20,
                itemOpacity: 0.75,
                symbolSize: 12,
                symbolShape: 'circle',
                symbolBorderColor: 'rgba(0, 0, 0, .5)',
                effects: [
                    {
                        on: 'hover',
                        style: {
                            itemBackground: 'rgba(0, 0, 0, .03)',
                            itemOpacity: 1
                        }
                    }
                ]
            }
        ]}
    />
)
